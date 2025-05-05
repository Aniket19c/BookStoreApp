using BookStore.Models.DTO.User;
using RepositoryLayer.DTO;
using NLog;
using Business.Interface;
using Repository.DTO;
using RepositoryLayer.Interfaces;

namespace BookStore.BusinessLayer.Services
{
    public class UserBLImpl : IUserBL
    {
        private readonly IUserRL _userRL;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public UserBLImpl(IUserRL userRL)
        {
            _userRL = userRL;
        }

        public async Task<ResponseDto<string>> RegisterUserAsync(UserRequestDto request)
        {
            _logger.Info("Calling RegisterUserAsync in UserBLImpl");
            return await _userRL.RegisterUserAsync(request);
        }

        public async Task<ResponseDto<string>> DeleteUserAsync(string email)
        {
            _logger.Info("Calling DeleteUserAsync in UserBLImpl");
            return await _userRL.DeleteUserAsync(email);
        }

        public async Task<ResponseDto<List<UserResponseDto>>> GetAllUsersAsync()
        {
            _logger.Info("Calling GetAllUsersAsync in UserBLImpl");
            return await _userRL.GetAllUsersAsync();
        }

        public async Task<ResponseDto<LoginResponseDto>> UserLoginAsync(LoginDto request)
        {
            _logger.Info("Calling UserLoginAsync in UserBLImpl");
            return await _userRL.UserLoginAsync(request);
        }

        public async Task<ResponseDto<string>> ForgetPasswordAsync(string email)
        {
            _logger.Info("Calling ForgetPasswordAsync in UserBLImpl");
            return await _userRL.ForgetPasswordAsync(email);
        }

        public async Task<ResponseDto<string>> ResetPasswordAsync(ResetPasswordDto dto, string email)
        {
            _logger.Info("Calling ResetPasswordAsync in UserBLImpl");
            return await _userRL.ResetPasswordAsync(dto, email);
        }
    }
}
