using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BookStore.Controllers;
using RepositoryLayer.DTO;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BookStore.Tests
{
    [TestFixture]
    public class CartControllerTests
    {
        private Mock<ICartBL> _mockCartBL;
        private Mock<ILogger<CartController>> _mockLogger;
        private CartController _cartController;

        [SetUp]
        public void Setup()
        {
            _mockCartBL = new Mock<ICartBL>();
            _mockLogger = new Mock<ILogger<CartController>>();
            _cartController = new CartController(_mockCartBL.Object, _mockLogger.Object);

            // Mocking user with UserId claim
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId", "1")
            }, "mock"));

            _cartController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task AddCart_ShouldReturnOk_WhenItemIsAdded()
        {
            var cartDto = new CartDto
            {
                BookId = 1,
                Quantity = 2
            };

            _mockCartBL.Setup(x => x.AddCartAsync(cartDto)).ReturnsAsync(1);

            var result = await _cartController.AddCart(cartDto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Item added to cart. CartId: 1", okResult.Value);
        }

        [Test]
        public async Task AddCart_ShouldReturnBadRequest_WhenItemIsNotAdded()
        {
            var cartDto = new CartDto
            {
                BookId = 1,
                Quantity = 2
            };

            _mockCartBL.Setup(x => x.AddCartAsync(cartDto)).ReturnsAsync(0);

            var result = await _cartController.AddCart(cartDto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Failed to add item to cart.", badRequestResult.Value);
        }

        [Test]
        public async Task GetCart_ShouldReturnOk_WhenItemsAreRetrieved()
        {
            var cartItems = new List<CartResponse>
            {
                new CartResponse { BookId = 1, Quantity = 2 },
                new CartResponse { BookId = 2, Quantity = 3 }
            };

            _mockCartBL.Setup(x => x.GetCartByUserIdAsync()).ReturnsAsync(cartItems);

            var result = await _cartController.GetCart();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(cartItems, okResult.Value);
        }

        [Test]
        public async Task RemoveCartItem_ShouldReturnOk_WhenItemIsRemoved()
        {
            var cartId = 1;
            _mockCartBL.Setup(x => x.UnCartAsync(cartId)).ReturnsAsync(true);

            var result = await _cartController.RemoveCartItem(cartId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Item removed from cart.", okResult.Value);
        }

        [Test]
        public async Task RemoveCartItem_ShouldReturnNotFound_WhenItemIsNotFound()
        {
            var cartId = 1;
            _mockCartBL.Setup(x => x.UnCartAsync(cartId)).ReturnsAsync(false);

            var result = await _cartController.RemoveCartItem(cartId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Item not found in cart.", notFoundResult.Value);
        }

        [Test]
        public async Task UpdateCartOrder_ShouldReturnOk_WhenOrderStatusIsUpdated()
        {
            var cartId = 1;
            var isOrdered = true;

            _mockCartBL.Setup(x => x.UpdateCartOrderAsync(cartId, isOrdered)).ReturnsAsync(true);

            var result = await _cartController.UpdateCartOrder(cartId, isOrdered);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Cart item order status updated to True", okResult.Value);
        }

        [Test]
        public async Task UpdateCartOrder_ShouldReturnNotFound_WhenCartItemIsNotFound()
        {
            var cartId = 1;
            var isOrdered = true;

            _mockCartBL.Setup(x => x.UpdateCartOrderAsync(cartId, isOrdered)).ReturnsAsync(false);

            var result = await _cartController.UpdateCartOrder(cartId, isOrdered);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Cart item not found.", notFoundResult.Value);
        }

        [Test]
        public async Task UpdateCartQuantity_ShouldReturnOk_WhenQuantityIsUpdated()
        {
            var cartId = 1;
            var quantity = 5;

            _mockCartBL.Setup(x => x.UpdateCartQuantityAsync(cartId, quantity)).ReturnsAsync(true);

            var result = await _cartController.UpdateCartQuantity(cartId, quantity);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Cart item quantity updated to 5", okResult.Value);
        }

        [Test]
        public async Task UpdateCartQuantity_ShouldReturnNotFound_WhenCartItemIsNotFound()
        {
            var cartId = 1;
            var quantity = 5;

            _mockCartBL.Setup(x => x.UpdateCartQuantityAsync(cartId, quantity)).ReturnsAsync(false);

            var result = await _cartController.UpdateCartQuantity(cartId, quantity);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Cart item not found.", notFoundResult.Value);
        }
    }
}
