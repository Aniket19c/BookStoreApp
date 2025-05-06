using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models.Entities.Cart;
using BookStore.Models.Entities.Order;
using BookStore.Models.Entities.Wishlist;

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

        public ICollection<CartEntity> Carts { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
        public ICollection<WishlistEntity> WishLists { get; set; }
    }
}
