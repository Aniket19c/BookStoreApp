using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.DTO
{
    public class CartDto
    {
        public int BookId { get; set; }
        public int Quantity { get; set; } = 1;
    }

}
