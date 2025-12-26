using LoveShop.Models;
using Microsoft.EntityFrameworkCore;

namespace LoveShop.Persistence
{
    public class LoveShopDbContext : DbContext
    {
        public DbSet<Product> Products { get; init; }
        public DbSet<Category> Categories { get; init; }
        public DbSet<ProductCategory> ProductCategories { get; init; }
        public DbSet<Customer> Customers { get; init; }
        public DbSet<Cart> Carts { get; init; }
        public DbSet<ProductInCart> ProductsInCarts { get; init; }
        public DbSet<Supplier> Suppliers { get; init; }
        public DbSet<Order> Orders { get; init; }
        public DbSet<OrderItem> OrderItems { get; init; }

        public LoveShopDbContext(DbContextOptions<LoveShopDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new ProductInCartConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        }
    }
}