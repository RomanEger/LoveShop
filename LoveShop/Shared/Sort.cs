using System.Linq.Expressions;

namespace LoveShop.Shared
{
	public sealed record Sort<T, K>(Expression<Func<T, K>> KeySelector);
}