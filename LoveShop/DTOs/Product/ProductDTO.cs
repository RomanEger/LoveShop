namespace LoveShop.DTOs.Product
{
	public sealed record ProductDTO(
		Guid Id,
		string Name,
		string Description,
		decimal Price,
		ICollection<Guid> CategoriesIds,
		byte[] RowVersion);
}