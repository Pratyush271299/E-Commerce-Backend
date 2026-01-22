using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class ShopperDBContext : DbContext
    {
        public ShopperDBContext(DbContextOptions<ShopperDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<AllProduct> AllProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopperDBContext).Assembly);
        }
    }
}
