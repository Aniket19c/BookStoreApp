using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Entities;
using NLog;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;
using System.Security.Claims;

public class CartBLImpl : ICartBL
{
    private readonly ICartRL _cartRL;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public CartBLImpl(ICartRL cartRL, IHttpContextAccessor httpContextAccessor)
    {
        _cartRL = cartRL;
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
            _logger.Error(ex, "Error while adding item to cart.");
            throw new Exception("Error while adding item to cart.");
        }
    }

    public async Task<List<CartResponse>> GetCartByUserIdAsync()
    {
        try
        {
            var userId = GetUserIdFromClaims();
            return await _cartRL.GetCartByUserIdAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error while retrieving cart items.");
            throw;
        }
    }

    public async Task<bool> UnCartAsync(int cartId)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            return await _cartRL.UnCartAsync(cartId, userId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error while uncaring item.");
            throw;
        }
    }

    public async Task<bool> UpdateCartOrderAsync(int cartId, bool isOrdered)
    {
        try
        {
            return await _cartRL.UpdateCartOrderAsync(cartId, isOrdered);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error while updating cart order.");
            throw;
        }
    }

    public async Task<bool> UpdateCartQuantityAsync(int cartId, int quantity)
    {
        try
        {
            var userId = GetUserIdFromClaims();
            return await _cartRL.UpdateCartQuantityAsync(cartId, quantity, userId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error while updating cart quantity.");
            throw;
        }
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
                _logger.Error("Invalid UserId format in claims: {0}", userIdClaim.Value);
                throw new UnauthorizedAccessException("Invalid UserId format.");
            }
        }

        _logger.Error("UserId claim not found in token.");
        throw new UnauthorizedAccessException("User not authorized.");
    }
}
