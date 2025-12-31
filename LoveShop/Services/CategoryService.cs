using LoveShop.Extensions;
using LoveShop.Models;
using LoveShop.Persistence;
using LoveShop.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LoveShop.Services
{
	public class CategoryService
	{
		private readonly LoveShopDbContext _loveShopDbContext;

		public CategoryService(LoveShopDbContext loveShopDbContext)
		{
			_loveShopDbContext = loveShopDbContext;
		}

		public async Task<Paginated<Category>> GetCategoriesAsync<T>(
			Filter<Category> filter,
			Sort<Category, T>? sort = null,
			CancellationToken cancellationToken = default)
		{
			var query = _loveShopDbContext.Categories.GetEntitiesAsync(filter, sort).AsNoTracking();

			var items = await query.ToListAsync(cancellationToken);

			var paginated = new Paginated<Category>(
				items, filter.PaginatedFilter.PageNumber, filter.PaginatedFilter.PageSize, items.Count);

			return paginated;
		}

		public async Task<Category?> GetCategoryAsync(
			Expression<Func<Category, bool>> condition,
			CancellationToken cancellationToken = default
		)
		{
			var item = await _loveShopDbContext.Categories
				.AsNoTracking()
				.FirstOrDefaultAsync(condition, cancellationToken);
			return item;
		}

		public async Task CreateProductAsync(Category category, CancellationToken cancellationToken = default)
		{
			await _loveShopDbContext.Categories.AddAsync(category, cancellationToken);
			await _loveShopDbContext.SaveChangesAsync(cancellationToken);
		}
	}
}