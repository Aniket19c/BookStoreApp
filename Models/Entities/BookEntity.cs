using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models;
using Models.Entities;

namespace Models.Entities
{
    public class BookEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public ICollection<WishlistEntity> Wishlists { get; set; }
    }
}
