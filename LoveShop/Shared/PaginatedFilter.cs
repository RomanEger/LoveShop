namespace LoveShop.Shared
{
	public sealed record PaginatedFilter<T>(
		int PageNumber,
		int PageSize);
}