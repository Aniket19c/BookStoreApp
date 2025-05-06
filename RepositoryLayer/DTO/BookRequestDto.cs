using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.DTO
{
    public class BookRequestDto
    {
        [Required]
        public string BookName { get; set; }
        public string Description { get; set; }

        [Required]
        public string AuthorName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string BookImage { get; set; } 
    }
}
