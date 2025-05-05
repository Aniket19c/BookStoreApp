using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.DTO
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
