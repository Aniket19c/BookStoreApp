using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Entities.Book
{
    public class CategoryEntity
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string? Description { get; set; }

        public ICollection<BookEntity>? Books { get; set; }
    }
}
