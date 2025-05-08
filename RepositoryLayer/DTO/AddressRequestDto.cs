using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enums;

namespace RepositoryLayer.DTO
{
    public class AddressRequestDto
    {
        public int AddressId { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public AddressTypes Type { get; set; }
        public string Name { get; set; }
        public long MobileNumber { get; set; }
    }

}
