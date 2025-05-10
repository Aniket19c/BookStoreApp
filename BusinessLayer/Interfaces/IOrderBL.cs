using RepositoryLayer.DTO;

namespace BusinessLayer.Interfaces
{
    public interface IOrderBL
    {
        Task<List<OrderResponse>> AddOrder(List<OrderRequestDto> requests, int userId);
        Task<List<OrderResponse>> GetOrder(int userId);
    }
}
