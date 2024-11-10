using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;

namespace ShoppingCart.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ShoppingCart.Models.Item> Item { get; set; } = default!;
        public DbSet<ShoppingCart.Models.Cart> Cart { get; set; } = default!;
        public DbSet<ShoppingCart.Models.CartItem> CartItem { get; set; } = default!;
        public DbSet<Purchase> Purchase { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define foreign key relationships and disable cascading delete on SellerId to prevent multiple cascade paths.
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Buyer)
                .WithMany()
                .HasForeignKey(p => p.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);  // Keeps cascade on Buyer

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Seller)
                .WithMany()
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.NoAction); // Disables cascade on Seller to avoid multiple cascade paths
        }
    }
}
