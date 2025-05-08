using Business.Interface;
using Model.Entities;
using Repository.Interface;

namespace Business.Implementation
{
    public class AddressBLImpl : IAddressBL
    {
        private readonly IAddressRL _addressRL;

        public AddressBLImpl(IAddressRL addressRL)
        {
            _addressRL = addressRL;
        }

        public async Task<bool> AddAddress(AddressEntity address)
        {
            return await _addressRL.AddAddress(address);
        }

        public async Task<bool> DeleteAddress(int addressId)
        {
            return await _addressRL.DeleteAddress(addressId);
        }

        public async Task<List<AddressEntity>> GetAllAddresses(int userId)
        {
            return await _addressRL.GetAllAddresses(userId);
        }

        public async Task<bool> UpdateAddress(AddressEntity address)
        {
            return await _addressRL.UpdateAddress(address);
        }
    }
}
