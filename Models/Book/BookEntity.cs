using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models.Entities.Book
{
    public class BookEntity
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        public string BookName { get; set; }  
        public string BookImage { get; set; }  
        public string Description { get; set; }  
        public string AuthorName { get; set; }  
        public int Quantity { get; set; }  
        public decimal Price { get; set; }  

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public CategoryEntity Category { get; set; }

        public ICollection<ReviewEntity>? Reviews { get; set; }
    }
}
