using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookStore.Models.Entities.User;

namespace BookStore.Models.Entities.Order
{
    public class OrderEntity
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; } 

        public string? ShippingAddress { get; set; }

        public UserEntity User { get; set; }

        public ICollection<OrderItemEntity>? OrderItems { get; set; }
    }
}
