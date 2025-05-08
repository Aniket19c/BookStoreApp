using Models.Entities;
using RepositoryLayer.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ICartRL
    {
        Task<int> AddCartAsync(CartEntity cart);
        Task<List<CartResponse>> GetCartByUserIdAsync(int userId);
        Task<bool> UnCartAsync(int cartId, int userId);
        Task<bool> UpdateCartOrderAsync(int cartId, bool isOrdered);
        Task<bool> UpdateCartQuantityAsync(int cartId, int quantity, int userId);
    }
}
