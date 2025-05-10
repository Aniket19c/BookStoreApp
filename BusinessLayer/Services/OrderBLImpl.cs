using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class OrderBLImpl : IOrderBL
    {
        private readonly IOrderRL _orderRL;

        public OrderBLImpl(IOrderRL orderRL)
        {
            _orderRL = orderRL;
        }


        public async Task<List<OrderResponse>> AddOrder(List<OrderRequestDto> requests, int userId)
        {
            try
            {
                return await _orderRL.AddOrder(requests, userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while placing the order: {ex.Message}", ex);
            }
        }

        public async Task<List<OrderResponse>> GetOrder(int userId)
        {
            try
            {
                return await _orderRL.GetOrder(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching orders: {ex.Message}", ex);
            }
        }

    }
}
