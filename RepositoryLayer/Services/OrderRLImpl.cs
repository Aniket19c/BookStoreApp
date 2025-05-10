using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RepositoryLayer.DTO;
using RepositoryLayer.Interfaces;
using BookStore.Models.Context;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class OrderRLImpl : IOrderRL
    {
        private readonly BookStoreDbContext _context;
        private readonly ILogger<OrderRLImpl> _logger;
        private readonly IDistributedCache _cache;

        public OrderRLImpl(BookStoreDbContext context, ILogger<OrderRLImpl> logger, IDistributedCache cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        public async Task<List<OrderResponse>> AddOrder(List<OrderRequestDto> requests, int userId)
        {
            var orderResponses = new List<OrderResponse>();

            foreach (var request in requests)
            {
                var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == request.BookId);
                if (book == null)
                {
                    _logger.LogWarning("Book not found for BookId: {BookId}", request.BookId);
                    throw new InvalidOperationException("Book not found.");
                }

                if (book.Quantity < request.Quantity)
                {
                    _logger.LogWarning("Requested quantity exceeds available stock for BookId: {BookId}", request.BookId);
                    throw new InvalidOperationException("Insufficient stock.");
                }

                var address = await _context.Addresses
                    .Where(a => a.AddressId == request.AddressId)
                    .Select(a => a.AddressLine)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(address))
                {
                    _logger.LogWarning("Address not found for AddressId: {AddressId}", request.AddressId);
                    throw new InvalidOperationException("Address not found.");
                }

                var order = new OrderEntity
                {
                    UserId = userId,
                    BookId = request.BookId,
                    AddressId = request.AddressId,
                    Quantity = request.Quantity,
                    TotalAmount = book.Price * request.Quantity,
                    Status = "Placed",
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = address
                };

                _context.Orders.Add(order);
                book.Quantity -= request.Quantity;

                await _context.SaveChangesAsync();

                var orderResponse = new OrderResponse
                {
                    OrderId = order.OrderId,
                    BookName = book.BookName,
                    Quantity = request.Quantity,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    OrderDate = order.OrderDate,
                    ShippingAddress = order.ShippingAddress
                };

                orderResponses.Add(orderResponse);

                var cacheKey = $"Book_{book.BookId}_Quantity";
                await _cache.SetStringAsync(cacheKey, book.Quantity.ToString());
            }

            return orderResponses;
        }

        public async Task<List<OrderResponse>> GetOrder(int userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.Book)
                    .ToListAsync();

                return orders.Select(o => new OrderResponse
                {
                    OrderId = o.OrderId,
                    BookName = o.Book?.BookName ?? "Unknown",
                    Quantity = o.Quantity,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    OrderDate = o.OrderDate,
                    ShippingAddress = o.ShippingAddress
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving orders for UserId: {UserId}", userId);
                throw;
            }
        }
    }
}
