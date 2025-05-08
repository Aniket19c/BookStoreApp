using BookStore.Models.Entities.Book;
using BookStore.Models.Entities.Cart;
using BookStore.Models.Entities.Order;
using BookStore.Models.Entities.User;
using BookStore.Models.Entities.Wishlist;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models.Context
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<CartEntity> Carts { get; set; }
        public DbSet<WishlistEntity> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CartEntity>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<OrderEntity>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<WishlistEntity>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<CartEntity>()
            .HasOne(c => c.Book)
                .WithMany(b => b.Carts)
                 .HasForeignKey(c => c.BookId)
                 .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
