using LoveShop.Shared;
using Microsoft.EntityFrameworkCore;

namespace LoveShop.Extensions
{
	public static class DbSetExtensions
	{
		extension<TEntity>(DbSet<TEntity> dbSet) where TEntity : class
		{
			public IQueryable<TEntity> GetEntitiesAsync<TK>(
				Filter<TEntity> filter,
				Sort<TEntity, TK>? sort = null
			)
			{
				var paginatedFilter = filter.PaginatedFilter;

				var query = dbSet
					.Paginate(paginatedFilter);

				query = sort is not null
					? query.OrderBy(sort.KeySelector)
					: query.Order();

				if (filter.Condition is not null)
				{
					query = query.Where(filter.Condition);
				}

				return query;
			}
		}
	}
}