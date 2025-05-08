using Models.Entities;

namespace BusinessLayer.Interfaces
{
    public interface IWishlistBL
    {
        Task<bool> AddToWishListAsync(WishlistEntity wishlist);
        Task<bool> RemoveFromWishListAsync(int wishlistId, int userId);
        Task<List<WishlistEntity>> GetAllWishlistItemsAsync(int userId);
    }
}

