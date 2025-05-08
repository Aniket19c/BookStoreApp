using Models.Entities;
using RepositoryLayer.DTO;

namespace BusinessLayer.Interfaces
{
    public interface ICartBL
    {
        
        Task<int> AddCartAsync(CartDto cartDto);
        Task<List<CartResponse>> GetCartByUserIdAsync();
        Task<bool> UnCartAsync(int cartId);
        Task<bool> UpdateCartOrderAsync(int cartId, bool isOrdered);
        Task<bool> UpdateCartQuantityAsync(int cartId, int quantity);
    }
}
