using RepositoryLayer.DTO;

namespace RepositoryLayer.Interfaces
{
    public interface IOrderRL
    {
        Task<List<OrderResponse>> GetOrder(int userId);
        Task<List<OrderResponse>> AddOrder(List<OrderRequestDto> requests, int userId);
    }
}

