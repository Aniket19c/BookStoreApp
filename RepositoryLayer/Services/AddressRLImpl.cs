using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Model.Entities;
using NLog;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BookStore.Models.Context;

namespace Repository.Implementation
{
    public class AddressRLImpl : IAddressRL
    {
        private readonly BookStoreDbContext _context;
        private readonly IDistributedCache _cache;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public AddressRLImpl(BookStoreDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<bool> AddAddress(AddressEntity address)
        {
            try
            {
                await _context.Addresses.AddAsync(address);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    await _cache.RemoveAsync(GetCacheKey(address.UserId));
                    _logger.Info($"Address added for user {address.UserId}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to add address");
                return false;
            }
        }

        public async Task<bool> DeleteAddress(int addressId)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(a => a.AddressId == addressId);
                if (address == null)
                {
                    _logger.Warn($"Address with ID {addressId} not found");
                    return false;
                }

                _context.Addresses.Remove(address);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    await _cache.RemoveAsync(GetCacheKey(address.UserId));
                    _logger.Info($"Address with ID {addressId} deleted");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to delete address");
                return false;
            }
        }

        public async Task<List<AddressEntity>> GetAllAddresses(int userId)
        {
            try
            {
                var cacheKey = GetCacheKey(userId);
                var cachedData = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    _logger.Info($"Cache hit for addresses of user {userId}");
                    return JsonSerializer.Deserialize<List<AddressEntity>>(cachedData);
                }

                var addresses = await _context.Addresses
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                if (addresses != null && addresses.Any())
                {
                    await _cache.SetStringAsync(
                        cacheKey,
                        JsonSerializer.Serialize(addresses),
                        new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) }
                    );
                    _logger.Info($"Cache set for addresses of user {userId}");
                }

                return addresses;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Failed to fetch addresses for user {userId}");
                return new List<AddressEntity>();
            }
        }

        public async Task<bool> UpdateAddress(AddressEntity address)
        {
            try
            {
                var existing = await _context.Addresses.FirstOrDefaultAsync(a => a.AddressId == address.AddressId);
                if (existing == null)
                {
                    _logger.Warn($"Address with ID {address.AddressId} not found for update");
                    return false;
                }

                existing.AddressLine = address.AddressLine;
                existing.City = address.City;
                existing.State = address.State;
                existing.Type = address.Type;
                existing.Name = address.Name;
                existing.MobileNumber = address.MobileNumber;

                _context.Addresses.Update(existing);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    await _cache.RemoveAsync(GetCacheKey(address.UserId));
                    _logger.Info($"Address with ID {address.AddressId} updated");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to update address");
                return false;
            }
        }

        private string GetCacheKey(int userId) => $"Address_User_{userId}";
    }
}
