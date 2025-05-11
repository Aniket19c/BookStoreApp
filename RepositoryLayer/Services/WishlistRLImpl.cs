using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models.Context;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using NLog;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class WishListRLImpl : IWishListRL
    {
        private readonly BookStoreDbContext _context;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WishListRLImpl(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToWishListAsync(WishlistEntity wishlist)
        {
            try
            {
                await _context.Wishlists.AddAsync(wishlist);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while adding item to wishlist.");
                throw new Exception("Error while adding item to wishlist.");
            }
        }

        public async Task<bool> RemoveFromWishListAsync(int wishlistId, int userId)
        {
            try
            {
                var entity = await _context.Wishlists
                    .FirstOrDefaultAsync(w => w.WishListId == wishlistId && w.UserId == userId);

                if (entity != null)
                {
                    _context.Wishlists.Remove(entity);
                    return await _context.SaveChangesAsync() > 0;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while removing item from wishlist.");
                return false;
            }
        }

        public async Task<List<WishlistEntity>> GetAllWishlistItemsAsync(int userId)
        {
            try
            {
                return await _context.Wishlists
                    .Include(w => w.Book)
                    .Where(w => w.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while fetching wishlist items.");
                throw new Exception("Error while fetching wishlist items.");
            }
        }
    }
}
