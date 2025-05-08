using Model.Entities;

namespace Business.Interface
{
    public interface IAddressBL
    {
        Task<bool> AddAddress(AddressEntity address);
        Task<bool> DeleteAddress(int addressId);
        Task<List<AddressEntity>> GetAllAddresses(int userId);
        Task<bool> UpdateAddress(AddressEntity address);
    }
}
