using BusinessLayer.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;  
using Models.Entities;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;

public class CartBLImpl : ICartBL
{
    private readonly ICartRL _cartRL;
    private readonly ILogger<CartBLImpl> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;  

    public CartBLImpl(ICartRL cartRL, ILogger<CartBLImpl> logger, IHttpContextAccessor httpContextAccessor)
    {
        _cartRL = cartRL;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;  
    }

    public async Task<int> AddCartAsync(CartDto cartDto)
    {
        try
        {
            var userId = GetUserIdFromClaims();  

            var cartEntity = new CartEntity
            {
                UserId = userId,
                BookId = cartDto.BookId,
                Quantity = cartDto.Quantity
            };

            return await _cartRL.AddCartAsync(cartEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding item to cart.");
            throw new Exception("Error while adding item to cart.");
        }
    }

    public async Task<List<CartResponse>> GetCartByUserIdAsync()
    {
        var userId = GetUserIdFromClaims();  
        return await _cartRL.GetCartByUserIdAsync(userId);
    }

    public async Task<bool> UnCartAsync(int cartId)
    {
        var userId = GetUserIdFromClaims(); 
        return await _cartRL.UnCartAsync(cartId, userId);
    }

    public async Task<bool> UpdateCartOrderAsync(int cartId, bool isOrdered)
    {
        return await _cartRL.UpdateCartOrderAsync(cartId, isOrdered);
    }

    public async Task<bool> UpdateCartQuantityAsync(int cartId, int quantity)
    {
        var userId = GetUserIdFromClaims();  
        return await _cartRL.UpdateCartQuantityAsync(cartId, quantity, userId);
    }

    private int GetUserIdFromClaims()
    {
        
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId");

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
}
