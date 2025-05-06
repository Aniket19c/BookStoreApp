using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models.Entities.Book;

namespace BookStore.Models.Entities.Cart
{
    [Table("CartItem")]
    public class CartItemEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }

        [Required]
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public CartEntity Cart { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public BookEntity Book { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
