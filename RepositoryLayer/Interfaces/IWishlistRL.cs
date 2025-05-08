using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;

namespace RepositoryLayer.Interfaces
{
    public interface IWishListRL
    {
        Task<bool> AddToWishListAsync(WishlistEntity wishlist);
        Task<bool> RemoveFromWishListAsync(int wishlistId, int userId);
        Task<List<WishlistEntity>> GetAllWishlistItemsAsync(int userId);
    }
}
