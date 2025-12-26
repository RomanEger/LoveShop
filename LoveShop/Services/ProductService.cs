using LoveShop.Models;
using LoveShop.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LoveShop.Services
{
    public class ProductService
    {
        private readonly LoveShopDbContext _loveShopDbContext;

        public ProductService(LoveShopDbContext context)
        {
            _loveShopDbContext = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _loveShopDbContext.Products
                .AsNoTracking()
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetProductAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _loveShopDbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task CreateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _loveShopDbContext.Products.AddAsync(product, cancellationToken);
            await _loveShopDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}