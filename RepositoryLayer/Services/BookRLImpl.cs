using BookStore.Models.DTO;
using BookStore.Models.Entities.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NLog;
using Repository.Helper.CustomExceptions;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;

public class BookRLImpl : IBookRL
{
    private readonly BookStoreDbContext _context;
    private readonly IDistributedCache _cache;
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public BookRLImpl(BookStoreDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<ResponseDto<string>> AddBookAsync(BookRequestDto request)
    {
        try
        {
            _logger.Info("AddBookAsync called");

            var book = new BookEntity
            {
                BookName = request.BookName,
                BookImage = request.BookImage,
                Description = request.Description,
                AuthorName = request.AuthorName,
                Quantity = request.Quantity,
                Price = request.Price
            };

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync("AllBooks");

            _logger.Info("Book added successfully and cache invalidated");

            return new ResponseDto<string> { success = true, message = "Book added successfully", data = null };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error adding book");
            throw;
        }
    }

    public async Task<ResponseDto<BookResponseDto>> GetBookByIdAsync(int bookId)
    {
        try
        {
            _logger.Info("GetBookByIdAsync called");

            string cacheKey = $"Book_{bookId}";
            var cachedBook = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedBook))
            {
                var book = JsonConvert.DeserializeObject<BookResponseDto>(cachedBook);
                _logger.Info("Book retrieved from cache");
                return new ResponseDto<BookResponseDto> { success = true, message = "Book fetched", data = book };
            }

            var bookEntity = await _context.Books.FindAsync(bookId);
            if (bookEntity == null)
            {
                _logger.Warn("Book not found");
                throw new BookNotFoundException();
            }

            var response = new BookResponseDto
            {
                BookName = bookEntity.BookName,
                AuthorName = bookEntity.AuthorName,
                Description = bookEntity.Description,
                Price = bookEntity.Price,
            };

            string serializedData = JsonConvert.SerializeObject(response);
            await _cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            _logger.Info("Book fetched from DB and cached");

            return new ResponseDto<BookResponseDto> { success = true, message = "Book fetched", data = response };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving book by ID");
            throw;
        }
    }

    public async Task<ResponseDto<List<BookResponseDto>>> GetAllBooksAsync()
    {
        try
        {
            _logger.Info("GetAllBooksAsync called");

            string cacheKey = "AllBooks";
            var cachedBooks = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedBooks))
            {
                var books = JsonConvert.DeserializeObject<List<BookResponseDto>>(cachedBooks);
                _logger.Info("Books retrieved from cache");
                return new ResponseDto<List<BookResponseDto>> { success = true, message = "Books fetched", data = books };
            }

            var bookEntities = await _context.Books.ToListAsync();
            var response = bookEntities.Select(book => new BookResponseDto
            {
                BookName = book.BookName,
                AuthorName = book.AuthorName,
                Description = book.Description,
                Price = book.Price,
            }).ToList();

            string serializedData = JsonConvert.SerializeObject(response);
            await _cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            _logger.Info("Books fetched from DB and cached");

            return new ResponseDto<List<BookResponseDto>> { success = true, message = "Books fetched", data = response };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error retrieving all books");
            throw;
        }
    }
}
