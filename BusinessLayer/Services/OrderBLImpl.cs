using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using NLog;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class OrderBLImpl : IOrderBL
    {
        private readonly IOrderRL _orderRL;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public OrderBLImpl(IOrderRL orderRL)
        {
            _orderRL = orderRL;
        }

        public async Task<List<OrderResponse>> AddOrder(List<OrderRequestDto> requests, int userId)
        {
            try
            {
                _logger.Info("AddOrder called for UserId: {0} with {1} request(s)", userId, requests?.Count ?? 0);
                var result = await _orderRL.AddOrder(requests, userId);
                _logger.Info("Order placed successfully for UserId: {0}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while placing the order for UserId: {0}", userId);
                throw new Exception($"Error occurred while placing the order: {ex.Message}", ex);
            }
        }

        public async Task<List<OrderResponse>> GetOrder(int userId)
        {
            try
            {
                _logger.Info("GetOrder called for UserId: {0}", userId);
                var result = await _orderRL.GetOrder(userId);
                _logger.Info("Retrieved {0} orders for UserId: {1}", result?.Count ?? 0, userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while fetching orders for UserId: {0}", userId);
                throw new Exception($"Error occurred while fetching orders: {ex.Message}", ex);
            }
        }
    }
}
