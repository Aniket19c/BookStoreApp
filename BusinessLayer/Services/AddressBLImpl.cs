using Business.Interface;
using Model.Entities;
using Repository.Interface;
using NLog;

namespace Business.Implementation
{
    public class AddressBLImpl : IAddressBL
    {
        private readonly IAddressRL _addressRL;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public AddressBLImpl(IAddressRL addressRL)
        {
            _addressRL = addressRL;
        }

        public async Task<bool> AddAddress(AddressEntity address)
        {
            try
            {
                return await _addressRL.AddAddress(address);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while adding address.");
                throw;
            }
        }

        public async Task<bool> DeleteAddress(int addressId)
        {
            try
            {
                return await _addressRL.DeleteAddress(addressId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while deleting address.");
                throw;
            }
        }

        public async Task<List<AddressEntity>> GetAllAddresses(int userId)
        {
            try
            {
                return await _addressRL.GetAllAddresses(userId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while retrieving addresses.");
                throw;
            }
        }

        public async Task<bool> UpdateAddress(AddressEntity address)
        {
            try
            {
                return await _addressRL.UpdateAddress(address);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while updating address.");
                throw;
            }
        }
    }
}
