using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.DTO;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartBL _cartBL;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartBL cartBL, ILogger<CartController> logger)
        {
            _cartBL = cartBL;
            _logger = logger;
        }

       
        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null)
            {
                if (int.TryParse(userIdClaim.Value, out var userId))
                {
                    return userId;
                }
                else
                {
                    _logger.LogError($"Invalid UserId format in claims: {userIdClaim.Value}");
                    throw new UnauthorizedAccessException("Invalid UserId format.");
                }
            }
            throw new UnauthorizedAccessException("User not authorized.");
        }

        
        [HttpPost("add")]
        public async Task<IActionResult> AddCart(CartDto cartDto)
        {
            try
            {
                int userId = GetUserIdFromClaims(); 
                var cartId = await _cartBL.AddCartAsync(cartDto); 
                if (cartId > 0)
                {
                    _logger.LogInformation($"Cart item added for UserId {userId}");
                    return Ok($"Item added to cart. CartId: {cartId}");
                }
                else
                {
                    return BadRequest("Failed to add item to cart.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding item to cart.");
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }

        
        [HttpGet("all")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                int userId = GetUserIdFromClaims(); 
                var cartItems = await _cartBL.GetCartByUserIdAsync();
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching cart items.");
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }

        
        [HttpDelete("remove/{cartId}")]
        public async Task<IActionResult> RemoveCartItem(int cartId)
        {
            try
            {
                int userId = GetUserIdFromClaims();
                var result = await _cartBL.UnCartAsync(cartId);
                if (result)
                {
                    _logger.LogInformation($"Item removed from cart for UserId {userId}. CartId: {cartId}");
                    return Ok("Item removed from cart.");
                }
                else
                {
                    return NotFound("Item not found in cart.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while removing item from cart.");
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }

        
        [HttpPut("update-order/{cartId}")]
        public async Task<IActionResult> UpdateCartOrder(int cartId, [FromBody] bool isOrdered)
        {
            try
            {
                var result = await _cartBL.UpdateCartOrderAsync(cartId, isOrdered);
                if (result)
                {
                    return Ok($"Cart item order status updated to {isOrdered}");
                }
                else
                {
                    return NotFound("Cart item not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating cart order status.");
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }

        
        [HttpPut("update-quantity/{cartId}")]
        public async Task<IActionResult> UpdateCartQuantity(int cartId, [FromBody] int quantity)
        {
            try
            {
                var result = await _cartBL.UpdateCartQuantityAsync(cartId, quantity);
                if (result)
                {
                    return Ok($"Cart item quantity updated to {quantity}");
                }
                else
                {
                    return NotFound("Cart item not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating cart quantity.");
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }
    }
}
