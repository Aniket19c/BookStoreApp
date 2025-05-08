using BusinessLayer.Interfaces;
using Microsoft.Extensions.Logging;
using Models.Entities;
using RepositoryLayer.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class WishListBLImpl : IWishlistBL
    {
        private readonly IWishListRL _wishlistRL;
        private readonly ILogger<WishListBLImpl> _logger;

        public WishListBLImpl(IWishListRL wishlistRL, ILogger<WishListBLImpl> logger)
        {
            _wishlistRL = wishlistRL;
            _logger = logger;
        }

        public async Task<bool> AddToWishListAsync(WishlistEntity Wishslist)
        {
            return await _wishlistRL.AddToWishListAsync(Wishslist);
        }

        public async Task<bool> RemoveFromWishListAsync(int wishlistId, int userId)
        {
            return await _wishlistRL.RemoveFromWishListAsync(wishlistId, userId);
        }

        public async Task<List<WishlistEntity>> GetAllWishlistItemsAsync(int userId)
        {
            return await _wishlistRL.GetAllWishlistItemsAsync(userId);
        }
    }
}
