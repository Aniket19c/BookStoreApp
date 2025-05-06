using BookStore.Models.DTO;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.Controllers
{
   
    [ApiController]
   [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookBL _bookBL;
        public BookController(IBookBL bookBL)
        {
            _bookBL = bookBL;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBook([FromBody] BookRequestDto request)
        {
            var response = await _bookBL.AddBookAsync(request);
            if (response.success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var response = await _bookBL.GetBookByIdAsync(id);
            if (response.success)
                return Ok(response);
            return NotFound(response);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllBooks()
        {
            var response = await _bookBL.GetAllBooksAsync();
            return Ok(response);
        }
    }

}
