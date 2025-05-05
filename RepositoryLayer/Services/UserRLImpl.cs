using System;
using System.Threading.Tasks;
using BookStore.Models.DTO.User;
using BookStore.Models.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using Repository.Helper;
using Repository.Helper.CustomExceptions;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class UserRLImpl : IUserRL
    {
        private readonly IConfiguration _configuration;
        private readonly BookStoreDbContext _context;
        private readonly IDistributedCache _cache;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public UserRLImpl(BookStoreDbContext context, IConfiguration configuration, IDistributedCache cache)
        {
            _context = context;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<ResponseDto<string>> RegisterUserAsync(UserRequestDto request)
        {
            try
            {
                _logger.Info("RegisterUserAsync called");

                string cacheKey = $"UserByEmail_{request.Email}";
                string cachedUser = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedUser))
                {
                    _logger.Warn("User already exists (from cache)");
                    throw new UserAlreadyExistsException();
                }

                var user = await _context.Users.SingleOrDefaultAsync(e => e.Email == request.Email);
                if (user != null)
                {
                    _logger.Warn("User already exists (from DB)");
                    throw new UserAlreadyExistsException();
                }

                var userEntity = new UserEntity
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PasswordHash = PasswordHelper.HashPassword(request.Password),
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Role = request.Role,
                };

                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();

                await _cache.RemoveAsync("AllUsers");
                
                string serializedUser = JsonConvert.SerializeObject(userEntity);
                await _cache.SetStringAsync(cacheKey, serializedUser, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                });

                _logger.Info("User registered and cached successfully");

                return new ResponseDto<string>
                {
                    success = true,
                    message = "User registered successfully",
                    data = null
                };
            }
            catch (UserAlreadyExistsException ex)
            {
                _logger.Warn(ex, "User already exists exception");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception occurred during user registration");
                throw;
            }
        }
    }
}
