using Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace Model.DTOs
{
    public class AddressDto
    {
        [Required]
        public string AddressLine { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public AddressTypes Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public long MobileNumber { get; set; }
    }
}
