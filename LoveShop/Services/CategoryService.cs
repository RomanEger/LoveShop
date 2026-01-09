using LoveShop.DTOs.Category;
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

		public async Task<Paginated<CategoryDTO>> GetCategoriesAsync<T>(
			Filter<Category> filter,
			Sort<Category, T>? sort = null,
			CancellationToken cancellationToken = default)
		{
			var query = _loveShopDbContext.Categories.GetEntitiesAsync(filter, sort)
				.Select(category => category.ToDTO())
				.AsNoTracking();

			var items = await query.ToListAsync(cancellationToken);

			var paginated = new Paginated<CategoryDTO>(
				items, filter.PaginatedFilter.PageNumber, filter.PaginatedFilter.PageSize, items.Count);

			return paginated;
		}

		public async Task<CategoryDTO?> GetCategoryAsync(
			Expression<Func<Category, bool>> condition,
			CancellationToken cancellationToken = default)
		{
			var item = await _loveShopDbContext.Categories
				.AsNoTracking()
				.Where(condition)
				.Select(category => category.ToDTO())
				.SingleOrDefaultAsync(cancellationToken);
			return item;
		}

		public async Task<CategoryDTO> CreateCategoryAsync(
			CategoryCreateDTO categoryCreateDTO,
			CancellationToken cancellationToken = default)
		{
			var parentCategory = categoryCreateDTO.ParentCategoryId is null
				? null
				: await _loveShopDbContext.Categories.SingleOrDefaultAsync(
					c => c.Id == categoryCreateDTO.ParentCategoryId,
					cancellationToken);

			var category = new Category { Name = categoryCreateDTO.Name, ParentCategory = parentCategory };

			await _loveShopDbContext.Categories.AddAsync(category, cancellationToken);
			await _loveShopDbContext.SaveChangesAsync(cancellationToken);

			return category.ToDTO();
		}

		public async Task<CategoryDTO?> UpdateCategoryAsync(
			CategoryUpdateDTO categoryUpdateDTO,
			CancellationToken cancellationToken = default)
		{
			var category = await _loveShopDbContext.Categories
				.Include(c => c.ChildCategories)
				.SingleOrDefaultAsync(c => c.Id == categoryUpdateDTO.Id, cancellationToken);
			if (category is null)
			{
				return null;
			}

			category.Name = categoryUpdateDTO.Name;
			category.ParentCategoryId = categoryUpdateDTO.ParentCategoryId;

			await _loveShopDbContext.SaveChangesAsync(cancellationToken);

			var categoryDTO = category.ToDTO();

			return categoryDTO;
		}

		public async Task DeleteAsync(
			Category deleteEntity,
			CancellationToken cancellationToken = default)
		{
			_loveShopDbContext.Categories.Remove(deleteEntity);
			await _loveShopDbContext.SaveChangesAsync(cancellationToken);
		}

		public async Task DeleteAsync(
			Expression<Func<Category, bool>> condition,
			CancellationToken cancellationToken = default)
		{
			var item = await _loveShopDbContext.Categories
				.SingleOrDefaultAsync(condition, cancellationToken);
			if (item is not null)
			{
				await DeleteAsync(item, cancellationToken);
			}
		}
	}
}