using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryLayer.DTO;
using System.Security.Claims;

namespace BookStoreApp.Controllers
{
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
                _logger.LogInformation($"UserId {userId} successfully retrieved from claims.");
                return userId;
            }

            _logger.LogError("User not authorized, UserId claim is missing or invalid.");
            throw new UnauthorizedAccessException("User not authorized.");
        }

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
