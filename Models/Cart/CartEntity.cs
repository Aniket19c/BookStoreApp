using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models.Entities.Book;
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

        [Required]
        [Column("BookId")]  
        public int BookId { get; set; }
        public BookEntity Book { get; set; }
        public int Quantity { get; set; } = 1;
        public bool IsOrdered { get; set; } = false;
        public bool IsUnCarted { get; set; } = false;
    }
}
