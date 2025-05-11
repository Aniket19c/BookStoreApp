using BookStore.Models.DTO;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.Controllers
{
    /// <summary>
    /// Controller responsible for handling book-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookBL _bookBL;

        /// <summary>
        /// Constructor to inject the book business layer.
        /// </summary>
        /// <param name="bookBL">Business layer interface for book operations</param>
        public BookController(IBookBL bookBL)
        {
            _bookBL = bookBL;
        }

        /// <summary>
        /// Adds a new book to the system.
        /// </summary>
        /// <param name="request">Book details</param>
        /// <returns>Success or failure response</returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddBook([FromBody] BookRequestDto request)
        {
            var response = await _bookBL.AddBookAsync(request);
            if (response.success)
                return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Retrieves a book by its ID.
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <returns>Book details if found, otherwise 404</returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var response = await _bookBL.GetBookByIdAsync(id);
            if (response.success)
                return Ok(response);
            return NotFound(response);
        }

        /// <summary>
        /// Retrieves a list of all books.
        /// </summary>
        /// <returns>List of all books</returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllBooks()
        {
            var response = await _bookBL.GetAllBooksAsync();
            return Ok(response);
        }
    }
}
