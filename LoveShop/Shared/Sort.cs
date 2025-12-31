using System.Linq.Expressions;

namespace LoveShop.Shared
{
	public sealed record Sort<T, TK>(Expression<Func<T, TK>> KeySelector);
}