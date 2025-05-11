using BusinessLayer.Interfaces;
using Models.Entities;
using NLog;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class WishListBLImpl : IWishlistBL
    {
        private readonly IWishListRL _wishlistRL;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WishListBLImpl(IWishListRL wishlistRL)
        {
            _wishlistRL = wishlistRL;
        }

        public async Task<bool> AddToWishListAsync(WishlistEntity wishlist)
        {
            try
            {
                _logger.Info("AddToWishListAsync called for UserId: {0}, BookId: {1}", wishlist.UserId, wishlist.BookId);
                var result = await _wishlistRL.AddToWishListAsync(wishlist);
                _logger.Info("Wishlist item added: {0}", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while adding to wishlist for UserId: {0}", wishlist.UserId);
                throw;
            }
        }

        public async Task<bool> RemoveFromWishListAsync(int wishlistId, int userId)
        {
            try
            {
                _logger.Info("RemoveFromWishListAsync called for WishlistId: {0}, UserId: {1}", wishlistId, userId);
                var result = await _wishlistRL.RemoveFromWishListAsync(wishlistId, userId);
                _logger.Info("Wishlist item removed: {0}", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while removing wishlist item for WishlistId: {0}, UserId: {1}", wishlistId, userId);
                throw;
            }
        }

        public async Task<List<WishlistEntity>> GetAllWishlistItemsAsync(int userId)
        {
            try
            {
                _logger.Info("GetAllWishlistItemsAsync called for UserId: {0}", userId);
                var result = await _wishlistRL.GetAllWishlistItemsAsync(userId);
                _logger.Info("Retrieved {0} wishlist items for UserId: {1}", result?.Count ?? 0, userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while retrieving wishlist items for UserId: {0}", userId);
                throw;
            }
        }
    }
}
