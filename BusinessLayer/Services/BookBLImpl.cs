using BookStore.Models.DTO;
using BusinessLayer.Interfaces;
using Microsoft.Extensions.Logging;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class BookBLImpl : IBookBL
    {
        private readonly IBookRL _bookRL;
        private readonly ILogger<BookBLImpl> _logger;

        public BookBLImpl(IBookRL bookRL, ILogger<BookBLImpl> logger)
        {
            _bookRL = bookRL;
            _logger = logger;
        }

        public async Task<ResponseDto<string>> AddBookAsync(BookRequestDto request)
        {
            try
            {
                _logger.LogInformation("AddBookAsync called ");

                var response = await _bookRL.AddBookAsync(request);

                if (response.success)
                {
                    _logger.LogInformation("Book added successfully");
                }
                else
                {
                    _logger.LogWarning("Failed to add book");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding book");
                throw;
            }
        }

        public async Task<ResponseDto<BookResponseDto>> GetBookByIdAsync(int bookId)
        {
            try
            {
                _logger.LogInformation("GetBookByIdAsync called with BookId: {BookId}", bookId);

                var response = await _bookRL.GetBookByIdAsync(bookId);

                if (response.success)
                {
                    _logger.LogInformation("Book retrieved successfully with BookId: {BookId}", bookId);
                }
                else
                {
                    _logger.LogWarning("No book found for BookId: {BookId}", bookId);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving book with BookId: {BookId}", bookId);
                throw;
            }
        }

        public async Task<ResponseDto<List<BookResponseDto>>> GetAllBooksAsync()
        {
            try
            {
                _logger.LogInformation("GetAllBooksAsync called");

                var response = await _bookRL.GetAllBooksAsync();

                if (response.success)
                {
                    _logger.LogInformation("Retrieved {Count} books", response.data?.Count ?? 0);
                }
                else
                {
                    _logger.LogWarning("No books found");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all books");
                throw;
            }
        }
    }
}
