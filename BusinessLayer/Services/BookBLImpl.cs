using BookStore.Models.DTO;
using BusinessLayer.Interfaces;
using NLog;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class BookBLImpl : IBookBL
    {
        private readonly IBookRL _bookRL;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public BookBLImpl(IBookRL bookRL)
        {
            _bookRL = bookRL;
        }

        public async Task<ResponseDto<BookResponseDto>> AddBookAsync(BookRequestDto request)
        {
            try
            {
                _logger.Info("AddBookAsync called");

                var response = await _bookRL.AddBookAsync(request);

                if (response.success)
                {
                    _logger.Info("Book added successfully");
                }
                else
                {
                    _logger.Warn("Failed to add book");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while adding book");
                throw;
            }
        }

        public async Task<ResponseDto<BookResponseDto>> GetBookByIdAsync(int bookId)
        {
            try
            {
                _logger.Info("GetBookByIdAsync called with BookId: {0}", bookId);

                var response = await _bookRL.GetBookByIdAsync(bookId);

                if (response.success)
                {
                    _logger.Info("Book retrieved successfully with BookId: {0}", bookId);
                }
                else
                {
                    _logger.Warn("No book found for BookId: {0}", bookId);
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while retrieving book with BookId: {0}", bookId);
                throw;
            }
        }

        public async Task<ResponseDto<List<BookResponseDto>>> GetAllBooksAsync()
        {
            try
            {
                _logger.Info("GetAllBooksAsync called");

                var response = await _bookRL.GetAllBooksAsync();

                if (response.success)
                {
                    _logger.Info("Retrieved {0} books", response.data?.Count ?? 0);
                }
                else
                {
                    _logger.Warn("No books found");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while retrieving all books");
                throw;
            }
        }
    }
}
