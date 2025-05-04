using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models.Entities.User;

namespace BookStore.Models.Entities.Cart
{
    public class CartEntity
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        
        public UserEntity User { get; set; }

        public ICollection<CartItemEntity>? CartItems { get; set; }
    }
}
