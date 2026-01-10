using LoveShop.DTOs.Product;
using LoveShop.Extensions;
using LoveShop.Models;
using LoveShop.Persistence;
using LoveShop.Services.Contracts;
using LoveShop.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LoveShop.Services
{
	public class ProductService : IGenericCrudService<Product, ProductDTO, ProductCreateDTO, ProductUpdateDTO>
	{
		private readonly ILogger<ProductService> _logger;
		private readonly LoveShopDbContext _loveShopDbContext;

		public ProductService(LoveShopDbContext context, ILogger<ProductService> logger)
		{
			_loveShopDbContext = context;
			_logger = logger;
		}

		public async Task<Paginated<ProductDTO>> GetAsync<T>(
			Filter<Product> filter,
			Sort<Product, T>? sort = null,
			CancellationToken cancellationToken = default)
		{
			var paginatedFilter = filter.PaginatedFilter;

			var query = _loveShopDbContext.Products.GetEntitiesAsync(filter, sort)
				.Include(product => product.ProductCategories)
				.Select(product => product.ToDTO());

			var items = await query.ToListAsync(cancellationToken);

			var paginated = new Paginated<ProductDTO>(
				items, paginatedFilter.PageNumber, paginatedFilter.PageSize, items.Count);

			return paginated;
		}

		public async Task<ProductDTO?> FindAsync(
			Expression<Func<Product, bool>> condition,
			CancellationToken cancellationToken = default)
		{
			var query = _loveShopDbContext.Products
				.Include(product => product.ProductCategories)
				.Where(condition)
				.Select(product => product.ToDTO());

			var item = await query.SingleOrDefaultAsync(cancellationToken);

			return item;
		}

		public async Task<ProductDTO> CreateAsync(
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

			return product.ToDTO();
		}

		public Task<ProductDTO?> UpdateAsync(ProductUpdateDTO updateDto, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(Product deleteEntity, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(Expression<Func<Product, bool>> condition,
			CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}