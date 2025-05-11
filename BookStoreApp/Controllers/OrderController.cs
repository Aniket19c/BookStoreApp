using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryLayer.DTO;
using System.Security.Claims;

namespace BookStoreApp.Controllers
{
    /// <summary>
    /// Controller for handling order operations such as placing and retrieving orders.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderBL _orderBL;
        private readonly ILogger<OrdersController> _logger;

        /// <summary>
        /// Constructor to initialize the OrdersController with dependencies.
        /// </summary>
        /// <param name="orderBL">Order business layer for order-related operations</param>
        /// <param name="logger">Logger to log order-related events</param>
        public OrdersController(IOrderBL orderBL, ILogger<OrdersController> logger)
        {
            _orderBL = orderBL;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the UserId from the claims in the current HTTP context.
        /// </summary>
        /// <returns>UserId</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the UserId claim is missing or invalid</exception>
        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogInformation($"UserId {userId} successfully retrieved from claims.");
                return userId;
            }

            _logger.LogError("User not authorized, UserId claim is missing or invalid.");
            throw new UnauthorizedAccessException("User not authorized.");
        }

        /// <summary>
        /// Places a new order for the user.
        /// </summary>
        /// <param name="orderRequests">List of order requests to be placed</param>
        /// <returns>Result of the order placement</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] List<OrderRequestDto> orderRequests)
        {
            try
            {
                int userId = GetUserIdFromClaims();
                var orderResponses = await _orderBL.AddOrder(orderRequests, userId);
                return Ok(orderResponses);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access.");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while placing the order.");
                return StatusCode(500, $"Error occurred while placing the order: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all orders for the authenticated user.
        /// </summary>
        /// <returns>List of orders for the authenticated user</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                int userId = GetUserIdFromClaims();
                var orders = await _orderBL.GetOrder(userId);
                return Ok(orders);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access.");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching orders.");
                return StatusCode(500, $"Error occurred while fetching orders: {ex.Message}");
            }
        }
    }
}
