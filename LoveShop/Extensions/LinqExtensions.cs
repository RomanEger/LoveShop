using LoveShop.Shared;

namespace LoveShop.Extensions
{
	public static class LinqExtensions
	{
		public static IQueryable<TEntity> Paginate<TEntity>(
			this IQueryable<TEntity> source,
			PaginatedFilter<TEntity> paginatedFilter
		)
		{
			return source
				.Skip(paginatedFilter.PageNumber * paginatedFilter.PageSize)
				.Take(paginatedFilter.PageSize);
		}
	}
}