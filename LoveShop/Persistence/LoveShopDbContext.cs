using LoveShop.Models;
using Microsoft.EntityFrameworkCore;

namespace LoveShop.Persistence;

public class LoveShopDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
}