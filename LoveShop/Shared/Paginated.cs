namespace LoveShop.Shared
{
	public sealed record Paginated<T>(
		IReadOnlyList<T> Items,
		int Page,
		int Size,
		int Total
	);
}