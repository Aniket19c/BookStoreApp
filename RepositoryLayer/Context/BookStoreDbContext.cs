using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Models.Entities;

namespace BookStore.Models.Context
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<CartEntity> Carts { get; set; }
        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<WishlistEntity> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<CartEntity>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<CartEntity>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Carts)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Restrict);

           
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

           
            modelBuilder.Entity<WishlistEntity>()
                .HasOne(w => w.Book)
                .WithMany(b => b.Wishlists)
                .HasForeignKey(w => w.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
