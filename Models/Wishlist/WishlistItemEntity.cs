using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models.Entities.Book;

namespace BookStore.Models.Entities.Wishlist
{
    public class WishlistItemEntity
    {
        [Key]
        public int WishlistItemId { get; set; }

        [Required]
        [ForeignKey("Wishlist")]
        public int WishlistId { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }


        public WishlistEntity Wishlist { get; set; }

        public BookEntity Book { get; set; }
    }
}
