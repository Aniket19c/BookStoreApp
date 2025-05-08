using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using BookStore.Models.Entities.Cart;
using BookStore.Models.Entities.Order;
using BookStore.Models.Entities.Wishlist;

namespace BookStore.Models.Entities.User
{
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<CartEntity> Carts { get; set; } = new List<CartEntity>();
        public List<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
        public List<WishlistEntity> Wishlists { get; set; } = new List<WishlistEntity>();
    }
}
