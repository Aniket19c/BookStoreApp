using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Entities;

namespace Models.Entities
{
    [Table("Order")]
    public class OrderEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public int AddressId { get; set; }

        [ForeignKey("AddressId")]
        public AddressEntity Address { get; set; }

        public string? ShippingAddress { get; set; }

        [Required]
        public int BookId { get; set; }

        [ForeignKey("BookId")]
        public BookEntity Book { get; set; }

        [Required]
        public int Quantity { get; set; } 
    }
}
