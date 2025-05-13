using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryLayer.DTO;
using BookStoreApp.Controllers;
using Microsoft.AspNetCore.Http;

namespace BookStoreApp.Tests
{
    [TestFixture]
    public class OrdersControllerTests
    {
        private Mock<IOrderBL> _mockOrderBL;
        private Mock<ILogger<OrdersController>> _mockLogger;
        private OrdersController _ordersController;

        [SetUp]
        public void SetUp()
        {
            _mockOrderBL = new Mock<IOrderBL>();
            _mockLogger = new Mock<ILogger<OrdersController>>();
            _ordersController = new OrdersController(_mockOrderBL.Object, _mockLogger.Object);
        }

        [Test]
        public async Task AddOrder_ShouldReturnOk_WhenOrderIsPlacedSuccessfully()
        {
            var orderRequests = new List<OrderRequestDto>
            {
                new OrderRequestDto { BookId = 1, Quantity = 2 },
                new OrderRequestDto { BookId = 2, Quantity = 3 }
            };

            var orderResponses = new List<OrderResponse>
            {
                new OrderResponse
                {
                    OrderId = 1,
                    BookName = "Book1",
                    Quantity = 2,
                    TotalAmount = 30.00M,
                    Status = "Placed",
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = "Address 1"
                },
                new OrderResponse
                {
                    OrderId = 2,
                    BookName = "Book2",
                    Quantity = 3,
                    TotalAmount = 45.00M,
                    Status = "Placed",
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = "Address 2"
                }
            };

            _mockOrderBL
                .Setup(x => x.AddOrder(It.IsAny<List<OrderRequestDto>>(), It.IsAny<int>()))
                .ReturnsAsync(orderResponses);

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controllerContext.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(
                    new[] { new System.Security.Claims.Claim("UserId", "1") }
                )
            );
            _ordersController.ControllerContext = controllerContext;

            var result = await _ordersController.AddOrder(orderRequests);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(orderResponses, okResult?.Value);
        }

        [Test]
        public async Task GetOrders_ShouldReturnOk_WhenOrdersAreRetrievedSuccessfully()
        {
            var orders = new List<OrderResponse>
            {
                new OrderResponse
                {
                    OrderId = 1,
                    BookName = "Book1",
                    Quantity = 2,
                    TotalAmount = 30.00M,
                    Status = "Placed",
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = "Address 1"
                },
                new OrderResponse
                {
                    OrderId = 2,
                    BookName = "Book2",
                    Quantity = 3,
                    TotalAmount = 45.00M,
                    Status = "Shipped",
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = "Address 2"
                }
            };

            _mockOrderBL
                .Setup(x => x.GetOrder(It.IsAny<int>()))
                .ReturnsAsync(orders);

            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controllerContext.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(
                    new[] { new System.Security.Claims.Claim("UserId", "1") }
                )
            );
            _ordersController.ControllerContext = controllerContext;

            var result = await _ordersController.GetOrders();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(orders, okResult?.Value);
        }
    }
}
