using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models.Entities.User;

namespace BookStore.Models.Entities.Cart
{
    [Table("Cart")]
    public class CartEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        public ICollection<CartItemEntity> CartItems { get; set; } = new List<CartItemEntity>();
    }
}
