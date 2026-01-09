using LoveShop.DTOs.Product;
using LoveShop.Extensions;
using LoveShop.Models;
using LoveShop.Persistence;
using LoveShop.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LoveShop.Services
{
	public class ProductService
	{
		private readonly ILogger<ProductService> _logger;
		private readonly LoveShopDbContext _loveShopDbContext;

		public ProductService(LoveShopDbContext context, ILogger<ProductService> logger)
		{
			_loveShopDbContext = context;
			_logger = logger;
		}

		public async Task<Paginated<ProductDTO>> GetProductsAsync<T>(
			Filter<Product> filter,
			Sort<Product, T>? sort = null,
			CancellationToken cancellationToken = default)
		{
			var paginatedFilter = filter.PaginatedFilter;

			var query = _loveShopDbContext.Products.GetEntitiesAsync(filter, sort)
				.Include(p => p.ProductCategories)
				.Select(p => new ProductDTO(
					p.Id,
					p.Name,
					p.Description ?? string.Empty,
					p.Price,
					p.ProductCategories.Select(pc => pc.CategoryId).ToArray(),
					p.RowVersion));

			var items = await query.ToListAsync(cancellationToken);

			var paginated = new Paginated<ProductDTO>(
				items, paginatedFilter.PageNumber, paginatedFilter.PageSize, items.Count);

			return paginated;
		}

		public async Task<ProductDTO?> GetProductAsync(
			Expression<Func<ProductDTO, bool>> condition,
			CancellationToken cancellationToken = default)
		{
			var query = from products in _loveShopDbContext.Products
						join productCategories in _loveShopDbContext.ProductCategories
						on products.Id equals productCategories.ProductId
						select new ProductDTO(
							products.Id,
							products.Name,
							products.Description ?? string.Empty,
							products.Price,
							new List<Guid>(),
							products.RowVersion);
			var item = await query.AsNoTracking().Where(condition).SingleOrDefaultAsync(cancellationToken);
			return item;
		}

		public async Task CreateProductAsync(
			ProductCreateDTO productDTO,
			CancellationToken cancellationToken = default)
		{
			var categories = await _loveShopDbContext.Categories
				.Where(c => productDTO.CategoriesIDs.Contains(c.Id))
				.ToListAsync(cancellationToken: cancellationToken);

			ICollection<ProductCategory> productCategories = [.. categories.Select(c => new ProductCategory { Category = c })];

			var product = new Product
			{
				Name = productDTO.Name,
				Description = productDTO.Description,
				Price = productDTO.Price,
				ProductCategories = productCategories
			};

			await using var transaction = await _loveShopDbContext.Database.BeginTransactionAsync(cancellationToken);
			try
			{
				await _loveShopDbContext.Products.AddAsync(product, cancellationToken);
				await _loveShopDbContext.ProductCategories.AddRangeAsync(productCategories, cancellationToken);
				await _loveShopDbContext.SaveChangesAsync(cancellationToken);
				await transaction.CommitAsync(cancellationToken);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, "Error occured while creating product: {ErrorMessage}", exception.Message);
				await transaction.RollbackAsync(cancellationToken);
				throw;
			}
		}
	}
}