using BookStore.Models.Entities.Book;
using BookStore.Models.Entities.Cart;
using BookStore.Models.Entities.Order;
using BookStore.Models.Entities.User;
using BookStore.Models.Entities.Wishlist;
using Microsoft.EntityFrameworkCore;

public class BookStoreDbContext : DbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<BookEntity> Books { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<CartEntity> Carts { get; set; }
    public DbSet<CartItemEntity> CartItems { get; set; }
    public DbSet<WishlistEntity> Wishlists { get; set; }
    public DbSet<WishlistItemEntity> WishlistItems { get; set; }
}
