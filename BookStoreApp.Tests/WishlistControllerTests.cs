using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using BookStore.Controllers;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BookStore.Tests
{
    [TestFixture]
    public class WishListControllerTests
    {
        private Mock<IWishlistBL> _mockWishlistBL;
        private Mock<ILogger<WishListController>> _mockLogger;
        private WishListController _controller;

        [SetUp]
        public void Setup()
        {
            _mockWishlistBL = new Mock<IWishlistBL>();
            _mockLogger = new Mock<ILogger<WishListController>>();
            _controller = new WishListController(_mockWishlistBL.Object, _mockLogger.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId", "1")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task AddToWishlist_ShouldReturnOk_WhenBookIsAdded()
        {
            int bookId = 10;
            _mockWishlistBL.Setup(x => x.AddToWishListAsync(It.Is<WishlistEntity>(w => w.BookId == bookId && w.UserId == 1)))
                           .ReturnsAsync(true);

            var result = await _controller.AddToWishlist(bookId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual("Book added to wishlist", (result as OkObjectResult).Value);
        }

        [Test]
        public async Task AddToWishlist_ShouldReturnBadRequest_WhenBookNotAdded()
        {
            int bookId = 10;
            _mockWishlistBL.Setup(x => x.AddToWishListAsync(It.Is<WishlistEntity>(w => w.BookId == bookId && w.UserId == 1)))
                           .ReturnsAsync(false);

            var result = await _controller.AddToWishlist(bookId);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual("Failed to add to wishlist", (result as BadRequestObjectResult).Value);
        }

        [Test]
        public async Task AddToWishlist_ShouldReturnServerError_WhenExceptionThrown()
        {
            int bookId = 10;
            _mockWishlistBL.Setup(x => x.AddToWishListAsync(It.IsAny<WishlistEntity>()))
                           .ThrowsAsync(new Exception("Database failure"));

            var result = await _controller.AddToWishlist(bookId);

            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.IsTrue(objectResult.Value.ToString().Contains("Internal server error"));
        }

        [Test]
        public async Task RemoveFromWishlist_ShouldReturnOk_WhenRemoved()
        {
            int wishListId = 5;
            _mockWishlistBL.Setup(x => x.RemoveFromWishListAsync(wishListId, 1)).ReturnsAsync(true);

            var result = await _controller.RemoveFromWishlist(wishListId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual("Wishlist item removed", (result as OkObjectResult).Value);
        }

        [Test]
        public async Task RemoveFromWishlist_ShouldReturnNotFound_WhenItemDoesNotExist()
        {
            int wishListId = 5;
            _mockWishlistBL.Setup(x => x.RemoveFromWishListAsync(wishListId, 1)).ReturnsAsync(false);

            var result = await _controller.RemoveFromWishlist(wishListId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual("Wishlist item not found", (result as NotFoundObjectResult).Value);
        }

        [Test]
        public async Task RemoveFromWishlist_ShouldReturnServerError_WhenExceptionThrown()
        {
            _mockWishlistBL.Setup(x => x.RemoveFromWishListAsync(It.IsAny<int>(), 1))
                           .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.RemoveFromWishlist(1);

            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.IsTrue(objectResult.Value.ToString().Contains("Internal server error"));
        }

        [Test]
        public async Task GetAllWishlistItems_ShouldReturnOk_WithItems()
        {
            var expected = new List<WishlistEntity>
            {
                new WishlistEntity { WishListId = 1, BookId = 100, UserId = 1 },
                new WishlistEntity { WishListId = 2, BookId = 101, UserId = 1 }
            };

            _mockWishlistBL.Setup(x => x.GetAllWishlistItemsAsync(1)).ReturnsAsync(expected);

            var result = await _controller.GetAllWishlistItems();

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(expected, (result as OkObjectResult).Value);
        }

        [Test]
        public async Task GetAllWishlistItems_ShouldReturnServerError_WhenExceptionThrown()
        {
            _mockWishlistBL.Setup(x => x.GetAllWishlistItemsAsync(1)).ThrowsAsync(new Exception("Something went wrong"));

            var result = await _controller.GetAllWishlistItems();

            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.IsTrue(objectResult.Value.ToString().Contains("Internal server error"));
        }
    }
}
