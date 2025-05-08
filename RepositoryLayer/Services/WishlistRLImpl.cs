using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Entities;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer.Services
{
    public class WishListRLImpl : IWishListRL
    {
        private readonly BookStoreDbContext _context;
        private readonly ILogger<WishListRLImpl> _logger;

        public WishListRLImpl(BookStoreDbContext context, ILogger<WishListRLImpl> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddToWishListAsync(WishlistEntity wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveFromWishListAsync(int wishlistId, int userId)
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

        public async Task<List<WishlistEntity>> GetAllWishlistItemsAsync(int userId)
        {
            return await _context.Wishlists
                .Include(w => w.Book)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }
    }
}
