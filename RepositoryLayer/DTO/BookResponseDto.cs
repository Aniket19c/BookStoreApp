namespace BookStore.Models.DTO
{
    public class BookResponseDto
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookImage { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
