using BookStore.Models.DTO.User;
using Business.Interface;
using Microsoft.Extensions.Logging;
using Repository.DTO;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;

namespace Business.Service
{
    public class UserBLImpl : IUserBL
    {
        public IUserRL _user;
        private readonly ILogger<UserBLImpl> _logger;

        public UserBLImpl(IUserRL userRL, ILogger<UserBLImpl> logger)
        {
            _user = userRL;
            _logger = logger;
        }

        public async Task<ResponseDto<string>> RegisterUserAsync(UserRequestDto request)
        {
            _logger.LogInformation("RegisterUserAsync called.");
            try
            {
                return await _user.RegisterUserAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in RegisterUserAsync.");
                throw;
            }
        }

       
    }
}
