using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models.Entities.Book;

namespace BookStore.Models.Entities.Cart
{
    public class CartItemEntity
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        [ForeignKey("Cart")]
        public int CartId { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public CartEntity Cart { get; set; }

        public BookEntity Book { get; set; }
    }
}
