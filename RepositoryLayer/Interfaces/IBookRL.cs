using BookStore.Models.DTO;
using RepositoryLayer.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IBookRL
    {
       
        Task<ResponseDto<BookResponseDto>> AddBookAsync(BookRequestDto request);
        Task<ResponseDto<BookResponseDto>> GetBookByIdAsync(int bookId);
        Task<ResponseDto<List<BookResponseDto>>> GetAllBooksAsync();
    }
}
