using BookStore.Models.DTO;
using RepositoryLayer.DTO;

namespace BusinessLayer.Interfaces
{
    public interface IBookBL
    {
        Task<ResponseDto<string>> AddBookAsync(BookRequestDto request);
        Task<ResponseDto<BookResponseDto>> GetBookByIdAsync(int bookId);
        Task<ResponseDto<List<BookResponseDto>>> GetAllBooksAsync();

    }
}
