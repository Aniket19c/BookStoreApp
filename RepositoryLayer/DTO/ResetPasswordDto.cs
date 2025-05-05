using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.DTO
{
    public class ResetPasswordDto
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
