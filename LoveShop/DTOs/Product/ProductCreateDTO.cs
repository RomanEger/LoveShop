namespace LoveShop.DTOs.Product
{
	public sealed record ProductCreateDTO(
		string Name,
		string? Description,
		decimal Price,
		ICollection<Guid> CategoriesIDs);
}