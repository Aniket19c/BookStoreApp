using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models.Entities.User;

namespace BookStore.Models.Entities.Wishlist
{
    public class WishlistEntity
    {
        [Key]
        public int WishlistId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public UserEntity User { get; set; }

        //add BookId
    }
}
