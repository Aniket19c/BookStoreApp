using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.DTO.User
{
    public class UserRequestDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
    }
}
