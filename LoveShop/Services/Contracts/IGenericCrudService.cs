using LoveShop.Shared;
using System.Linq.Expressions;

namespace LoveShop.Services.Contracts
{
	public interface IGenericCrudService<TEntity, TDto, in TCreateDto, in TUpdateDto>
		where TEntity : class
		where TDto : class
		where TCreateDto : class
		where TUpdateDto : class
	{
		Task<Paginated<TDto>> GetAsync<TSortKey>(
			Filter<TEntity> filter,
			Sort<TEntity, TSortKey>? sort = null,
			CancellationToken cancellationToken = default);

		Task<TDto?> FindAsync(
			Expression<Func<TEntity, bool>> condition,
			CancellationToken cancellationToken = default);

		Task<TDto> CreateAsync(
			TCreateDto createDto,
			CancellationToken cancellationToken = default);

		Task<TDto?> UpdateAsync(
			TUpdateDto updateDto,
			CancellationToken cancellationToken = default);

		Task DeleteAsync(
			TEntity deleteEntity,
			CancellationToken cancellationToken = default);

		Task DeleteAsync(
			Expression<Func<TEntity, bool>> condition,
			CancellationToken cancellationToken = default);
	}
}