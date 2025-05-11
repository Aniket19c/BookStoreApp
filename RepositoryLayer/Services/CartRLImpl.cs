using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Models.Entities;
using NLog;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class CartRLImpl : ICartRL
    {
        private readonly BookStoreDbContext _context;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public CartRLImpl(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddCartAsync(CartEntity cart)
        {
            try
            {
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
                return cart.CartId;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while adding item to cart.");
                throw new Exception("Error while adding item to cart.");
            }
        }

        public async Task<List<CartResponse>> GetCartByUserIdAsync(int userId)
        {
            try
            {
                var cartItems = await _context.Carts
                    .Where(c => c.UserId == userId && !c.IsUnCarted)
                    .Include(c => c.Book)
                    .ToListAsync();

                return cartItems.Select(c => new CartResponse
                {
                    CartId = c.CartId,
                    BookId = c.BookId,
                    Quantity = c.Quantity,
                    BookName = c.Book.BookName,
                    BookPrice = c.Book.Price
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while fetching cart items.");
                throw new Exception("Error while fetching cart items.");
            }
        }

        public async Task<bool> UnCartAsync(int cartId, int userId)
        {
            try
            {
                var cartItem = await _context.Carts
                    .FirstOrDefaultAsync(c => c.CartId == cartId && c.UserId == userId);

                if (cartItem != null)
                {
                    cartItem.IsUnCarted = true;
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while uncaring item.");
                return false;
            }
        }

        public async Task<bool> UpdateCartOrderAsync(int cartId, bool isOrdered)
        {
            try
            {
                var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);

                if (cartItem != null)
                {
                    cartItem.IsOrdered = isOrdered;
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while updating order status.");
                return false;
            }
        }

        public async Task<bool> UpdateCartQuantityAsync(int cartId, int quantity, int userId)
        {
            try
            {
                var cartItem = await _context.Carts
                    .FirstOrDefaultAsync(c => c.CartId == cartId && c.UserId == userId);

                if (cartItem != null)
                {
                    cartItem.Quantity = quantity;
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while updating cart quantity.");
                return false;
            }
        }
    }
}
