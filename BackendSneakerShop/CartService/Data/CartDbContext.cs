using Microsoft.EntityFrameworkCore;
using CartService.Data.Entities;

namespace CartService.Data
{
    public class CartDbContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;

        public CartDbContext(DbContextOptions<CartDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
