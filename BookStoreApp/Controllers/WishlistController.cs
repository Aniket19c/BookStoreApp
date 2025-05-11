using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    /// <summary>
    /// Controller for managing users' wishlists.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WishListController : ControllerBase
    {
        private readonly IWishlistBL _wishListBL;
        private readonly ILogger<WishListController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishListController"/> class.
        /// </summary>
        /// <param name="wishListBL">The business logic layer for wishlist operations.</param>
        /// <param name="logger">Logger for logging errors and activities.</param>
        public WishListController(IWishlistBL wishListBL, ILogger<WishListController> logger)
        {
            _wishListBL = wishListBL;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the UserId from the claims in the JWT token.
        /// </summary>
        /// <returns>UserId as an integer.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if UserId is missing or invalid in the claims.</exception>
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

        /// <summary>
        /// Adds a book to the user's wishlist.
        /// </summary>
        /// <param name="bookId">The ID of the book to add.</param>
        /// <returns>Result of the operation, with success or failure message.</returns>
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

        /// <summary>
        /// Removes an item from the user's wishlist.
        /// </summary>
        /// <param name="wishListId">The ID of the wishlist item to remove.</param>
        /// <returns>Result of the operation, with success or failure message.</returns>
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

        /// <summary>
        /// Retrieves all items in the user's wishlist.
        /// </summary>
        /// <returns>List of wishlist items.</returns>
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
