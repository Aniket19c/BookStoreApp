using Model.Entities; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IAddressRL
    {
        Task<bool> AddAddress(AddressEntity address);
        Task<bool> DeleteAddress(int addressId);
        Task<List<AddressEntity>> GetAllAddresses(int userId);
        Task<bool> UpdateAddress(AddressEntity address);
    }
}
