using LoveShop.Models;
using LoveShop.Persistence;
using LoveShop.Shared;
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

        public async Task<Paginated<Product>> GetProductsAsync<T>(
            Filter<Product> filter,
            Sort<Product, T>? sort = null,
            CancellationToken cancellationToken = default
        )
        {
            var query = _loveShopDbContext.Products
                .AsNoTracking()
                .Skip(filter.PageNumber * filter.PageSize)
                .Take(filter.PageSize);

            query = sort is not null
                ? query.OrderBy(sort.KeySelector)
                : query.OrderBy(p => p.Name);

            if (filter.Condition is not null)
            {
                query = query.Where(filter.Condition);
            }

            var items = await query.ToListAsync(cancellationToken);
            var paginated = new Paginated<Product>(items, filter.PageNumber, filter.PageSize, items.Count);
            return paginated;
        }

        public async Task<Product?> GetProductAsync(
            Expression<Func<Product, bool>> condition,
            CancellationToken cancellationToken = default
        )
        {
            return await _loveShopDbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(condition, cancellationToken);
        }

        public async Task CreateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _loveShopDbContext.Products.AddAsync(product, cancellationToken);
            await _loveShopDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}