using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("WishList")]
    public class WishlistEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishListId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public BookEntity Book { get; set; }
    }
}
