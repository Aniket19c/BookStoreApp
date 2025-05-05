using BookStore.Models.DTO.User;
using Repository.DTO;
using RepositoryLayer.DTO;

namespace Business.Interface
{
    public interface IUserBL
    {
        Task<ResponseDto<string>> RegisterUserAsync(UserRequestDto request);
      


    }
}
