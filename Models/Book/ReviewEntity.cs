using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models.Entities.User;

namespace BookStore.Models.Entities.Book
{
    public class ReviewEntity
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public string? Comment { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public BookEntity Book { get; set; }

        public UserEntity User { get; set; }
    }
}
