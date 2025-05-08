using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;


namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WishListController : ControllerBase
    {
        private readonly IWishlistBL _wishListBL;
        private readonly ILogger<WishListController> _logger;

        public WishListController(IWishlistBL wishListBL, ILogger<WishListController> logger)
        {
            _wishListBL = wishListBL;
            _logger = logger;
        }

        private int GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            _logger.LogWarning("UserId claim missing or invalid.");
            throw new UnauthorizedAccessException("Invalid or missing UserId in claims.");
        }

        [HttpPost("add/{bookId}")]
        public async Task<IActionResult> AddToWishlist(int bookId)
        {
            try
            {
                int userId = GetUserIdFromClaims();

                var wishlist = new WishlistEntity
                {
                    BookId = bookId,
                    UserId = userId
                };

                bool result = await _wishListBL.AddToWishListAsync(wishlist);
                return result ? Ok("Book added to wishlist") : BadRequest("Failed to add to wishlist");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book to wishlist");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("remove/{wishListId}")]
        public async Task<IActionResult> RemoveFromWishlist(int wishListId)
        {
            try
            {
                int userId = GetUserIdFromClaims();
                bool result = await _wishListBL.RemoveFromWishListAsync(wishListId, userId);
                return result ? Ok("Wishlist item removed") : NotFound("Wishlist item not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing wishlist item");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllWishlistItems()
        {
            try
            {
                int userId = GetUserIdFromClaims();
                var result = await _wishListBL.GetAllWishlistItemsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wishlist");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
