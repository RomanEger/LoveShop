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
				.Select(product => new ProductDTO(
					product.Id,
					product.Name,
					product.Description ?? string.Empty,
					product.Price,
					product.ProductCategories.Select(pc => pc.CategoryId).ToArray(),
					product.RowVersion));

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

			var product = Product.Create(
				productDTO.Name,
				productDTO.Description ?? string.Empty,
				productDTO.Price,
				productCategories);

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

		public async Task<ProductDTO?> UpdateAsync(
			ProductUpdateDTO updateDto,
			CancellationToken cancellationToken = default)
		{
			var product = await _loveShopDbContext.Products
				.Include(product => product.ProductCategories)
				.SingleOrDefaultAsync(product => product.Id == updateDto.Id, cancellationToken);
			if (product is null)
			{
				return null;
			}

			var requestedCategoryIds = updateDto.CategoriesIds
				.Distinct()
				.ToArray();

			var existingCategoryIds = await _loveShopDbContext.Categories
				.Where(c => requestedCategoryIds.Contains(c.Id))
				.Select(c => c.Id)
				.ToArrayAsync(cancellationToken);

			if (existingCategoryIds.Length != requestedCategoryIds.Length)
			{
				return null;
			}

			var productCategoriesIds = product.ProductCategories.Select(x => x.CategoryId).ToHashSet();

			var addedCategoriesIds = requestedCategoryIds.Except(productCategoriesIds);

			var removedCategoriesIds = productCategoriesIds.Except(requestedCategoryIds);
			var removedCategories = product.ProductCategories
				.Where(productCategory => removedCategoriesIds.Contains(productCategory.CategoryId))
				.ToArray();

			product.Name = updateDto.Name;
			product.Description = updateDto.Description;
			product.Price = updateDto.Price;
			_loveShopDbContext.Entry(product)
				.Property(p => p.RowVersion)
				.OriginalValue = updateDto.RowVersion;

			foreach (var productCategory in addedCategoriesIds
				         .Select(categoryId => new ProductCategory { ProductId = product.Id, CategoryId = categoryId }))
			{
				product.ProductCategories.Add(productCategory);
			}

			foreach (var removedCategory in removedCategories)
			{
				product.ProductCategories.Remove(removedCategory);
			}

			await _loveShopDbContext.SaveChangesAsync(cancellationToken);

			return product.ToDTO();
		}

		public async Task DeleteAsync(Product deleteEntity, CancellationToken cancellationToken = default)
		{
			_loveShopDbContext.Products.Remove(deleteEntity);
			await _loveShopDbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task DeleteAsync(Expression<Func<Product, bool>> condition,
			CancellationToken cancellationToken = default)
		{
			var deleteEntity = await _loveShopDbContext.Products
				.SingleOrDefaultAsync(condition, cancellationToken);
			if (deleteEntity is not null)
			{
				await DeleteAsync(deleteEntity, cancellationToken);
			}
		}
	}
}