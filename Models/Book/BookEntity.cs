using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models.Entities.Book
{
    public class BookEntity
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public string? ISBN { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        public string? Description { get; set; }

        public string? Publisher { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string? CoverImageUrl { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public CategoryEntity Category { get; set; }

        public ICollection<ReviewEntity>? Reviews { get; set; }
    }
}
