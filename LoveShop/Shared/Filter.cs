using System.Linq.Expressions;

namespace LoveShop.Shared
{
	public sealed record Filter<T>(
		int PageNumber,
		int PageSize,
		Expression<Func<T, bool>>? Condition = null
	);
}