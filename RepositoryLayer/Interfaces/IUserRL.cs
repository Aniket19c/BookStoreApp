
using BookStore.Models.DTO.User;
using Repository.DTO;
using RepositoryLayer.DTO;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRL
    {
        Task<ResponseDto<string>> RegisterUserAsync(UserRequestDto requestDto);
        
    }
}
