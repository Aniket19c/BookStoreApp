using BookStore.Models.Context;
using BookStore.Models.DTO.User;
using BookStore.Models.Entities.User;
using ConsumerService;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using Repository.DTO;
using Repository.Helper;
using Repository.Helper.CustomExceptions;
using Repository_Layer.Helper;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRLImpl : IUserRL
    {
        private readonly BookStoreDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly JwtTokenHelper _jwtHelper;
        private readonly RabbitMqProducer _producer;  
        private readonly RabbitMqConsumer _consumer;

        public UserRLImpl(BookStoreDbContext context, IConfiguration configuration, IDistributedCache cache, JwtTokenHelper jwtHelper, RabbitMqProducer producer, RabbitMqConsumer consumer)
        {
            _context = context;
            _configuration = configuration;
            _cache = cache;
            _jwtHelper = jwtHelper;
            _producer = producer; 
            _consumer = consumer;
        }

        public async Task<ResponseDto<string>> RegisterUserAsync(UserRequestDto request)
        {
            try
            {
                _logger.Info("RegisterUserAsync called");

                string cacheKey = $"UserByEmail_{request.Email}";
                var cachedUser = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedUser))
                {
                    _logger.Warn("User already exists (from cache)");
                    throw new UserAlreadyExistsException();
                }

                var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
                if (existingUser != null)
                {
                    _logger.Warn("User already exists (from DB)");
                    throw new UserAlreadyExistsException();
                }

                var user = new UserEntity
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PasswordHash = PasswordHelper.HashPassword(request.Password),
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Role = request.Role
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                await _cache.RemoveAsync("AllUsers");

                string serializedUser = JsonConvert.SerializeObject(user);
                await _cache.SetStringAsync(cacheKey, serializedUser, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });

                _logger.Info("User registered successfully and cached");

                return new ResponseDto<string> { success = true, message = "User registered", data = null };
            }
            catch (UserAlreadyExistsException ex)
            {
                _logger.Warn(ex, "Duplicate registration attempt");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception during user registration");
                throw;
            }
        }

        public async Task<ResponseDto<string>> DeleteUserAsync(string email)
        {
            try
            {
                _logger.Info("DeleteUserAsync called");

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    _logger.Warn("User not found for deletion");
                    throw new UserNotFoundException();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                await _cache.RemoveAsync($"UserByEmail_{email}");
                await _cache.RemoveAsync("AllUsers");

                _logger.Info("User deleted and removed from cache");

                return new ResponseDto<string> { success = true, message = "User deleted successfully" };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error during user deletion");
                throw;
            }
        }

        public async Task<ResponseDto<List<UserResponseDto>>> GetAllUsersAsync()
        {
            try
            {
                _logger.Info("GetAllUsersAsync called");

                string cacheKey = "AllUsers";
                var cachedUsers = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedUsers))
                {
                    var users = JsonConvert.DeserializeObject<List<UserResponseDto>>(cachedUsers);
                    _logger.Info("Users retrieved from cache");
                    return new ResponseDto<List<UserResponseDto>> { success = true, message = "Users fetched", data = users };
                }

                var userEntities = await _context.Users.ToListAsync();
                var response = userEntities.Select(user => new UserResponseDto
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role
                }).ToList();

                string serializedData = JsonConvert.SerializeObject(response);
                await _cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });

                _logger.Info("Users fetched from DB and cached");

                return new ResponseDto<List<UserResponseDto>> { success = true, message = "Users fetched", data = response };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving all users");
                throw;
            }
        }

        public async Task<ResponseDto<LoginResponseDto>> UserLoginAsync(LoginDto request)
        {
            try
            {
                _logger.Info("UserLoginAsync called");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.email);
                if (user == null || !PasswordHelper.VerifyPassword(request.password, user.PasswordHash))
                {
                    _logger.Warn("Invalid login attempt");
                    throw new InvalidCredentialsException();
                }

             
                string token = _jwtHelper.GenerateToken(user.Email, user.UserId,user.Role);

                var loginResponse = new LoginResponseDto
                {
                    Role = user.Role,
                    Token = token 
                };

                _logger.Info("User logged in successfully with JWT");

                return new ResponseDto<LoginResponseDto> { success = true, message = "Login successful", data = loginResponse };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Login error");
                throw;
            }
        }


        public async Task<ResponseDto<string>> ForgetPasswordAsync(string email)
        {
            try
            {
                _logger.Info($"Sending token to: {email}");
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.Warn("Email not found");
                    throw new UserNotFoundException();
                }

               
                var token = _jwtHelper.GenerateToken(user.Email, user.UserId,user.Role);

               
                _producer.SendOtpQueue(email, token);

                _consumer.Consume();

                _logger.Info("Token sent successfully");

                return new ResponseDto<string>
                {
                    success = true,
                    message = "Token sent to your email address.",
                    data = null
                };
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "Exception in ForgetPasswordAsync");
                throw;
            }
        }


        public async Task<ResponseDto<string>> ResetPasswordAsync(ResetPasswordDto dto, string email)
        {
            try
            {
                _logger.Info("ResetPasswordAsync called");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.Warn("Reset password attempted for non-existing user");
                    throw new UserNotFoundException();
                }

                if (!PasswordHelper.VerifyPassword(dto.OldPassword, user.PasswordHash))
                {
                    _logger.Warn("Incorrect current password");
                    throw new IncorrectPasswordException();
                }


                if (dto.NewPassword != dto.ConfirmPassword)
                {
                    _logger.Warn("Password mismatch during reset");
                    throw new PasswordMismatchException();
                }


                user.PasswordHash = PasswordHelper.HashPassword(dto.NewPassword);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                _logger.Info("Password reset successful");

                return new ResponseDto<string> { success = true, message = "Password reset successful" };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in reset password");
                throw;
            }
        }



    }
}
