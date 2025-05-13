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

        public OrdersController(IOrderBL orderBL, ILogger<OrdersController> logger)
        {
            _orderBL = orderBL;
            _logger = logger;
        }

        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                _logger.LogInformation("UserId {UserId} successfully retrieved from claims.", userId);
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
                var result = await _orderBL.AddOrder(orderRequests, userId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access.");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while placing the order.");
                return StatusCode(500, $"An error occurred while placing the order: {ex.Message}");
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
                var result = await _orderBL.GetOrder(userId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access.");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching orders.");
                return StatusCode(500, $"An error occurred while fetching orders: {ex.Message}");
            }
        }
    }
}
