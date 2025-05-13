using BookStore.Models.DTO;
using BookStoreApp.Controllers;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RepositoryLayer.DTO;
using System.Threading.Tasks;

namespace BookStoreApp.Tests
{
    [TestFixture]
    public class BookControllerTests
    {
        private Mock<IBookBL> _mockBookBL;
        private BookController _bookController;

        [SetUp]
        public void Setup()
        {
            _mockBookBL = new Mock<IBookBL>();
            _bookController = new BookController(_mockBookBL.Object);
        }
        [Test]
        public async Task AddBook_ShouldReturnOk_WhenBookIsAdded()
        {
            var request = new BookRequestDto
            {
                BookName = "Test Book",
                BookImage = "test_image.jpg",
                Description = "Test description",
                AuthorName = "Test Author",
                Quantity = 10,
                Price = 15.99M
            };

            var expected = new ResponseDto<BookResponseDto>
            {
                success = true,
                message = "Book added successfully",
                data = new BookResponseDto
                {
                    BookId = 1,
                    BookName = "Test Book",
                    AuthorName = "Test Author",
                    Description = "Test description",
                    Price = 15.99M,
                    Quantity = 10,
                    BookImage = "test_image.jpg"
                }
            };

            _mockBookBL.Setup(x => x.AddBookAsync(It.IsAny<BookRequestDto>()))
                       .ReturnsAsync(expected);

            var result = await _bookController.AddBook(request);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var response = okResult.Value as ResponseDto<BookResponseDto>;
            Assert.IsTrue(response.success);
        }



        [Test]
        public async Task GetBookById_ShouldReturnOk_WhenBookExists()
        {
            var bookId = 1;
            var expected = new ResponseDto<BookResponseDto>
            {
                success = true,
                message = "Book found",
                data = new BookResponseDto
                {
                    BookId = bookId,
                    BookName = "Test Book",
                    BookImage = "test_image.jpg",
                    Description = "Test description",
                    AuthorName = "Test Author",
                    Quantity = 10,
                    Price = 15.99M
                }
            };

            _mockBookBL.Setup(x => x.GetBookByIdAsync(bookId))
                       .ReturnsAsync(expected);

            var result = await _bookController.GetBookById(bookId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsTrue(((ResponseDto<BookResponseDto>)okResult.Value).success);
        }

        [Test]
        public async Task GetAllBooks_ShouldReturnOk_WhenBooksExist()
        {
            var expected = new ResponseDto<List<BookResponseDto>>
            {
                success = true,
                message = "Books fetched successfully",
                data = new List<BookResponseDto>
                {
                    new BookResponseDto
                    {
                        BookId = 1,
                        BookName = "Test Book",
                        BookImage = "test_image.jpg",
                        Description = "Test description",
                        AuthorName = "Test Author",
                        Quantity = 10,
                        Price = 15.99M
                    }
                }
            };

            _mockBookBL.Setup(x => x.GetAllBooksAsync())
                       .ReturnsAsync(expected);

            var result = await _bookController.GetAllBooks();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsTrue(((ResponseDto<List<BookResponseDto>>)okResult.Value).success);
        }
    }
}
