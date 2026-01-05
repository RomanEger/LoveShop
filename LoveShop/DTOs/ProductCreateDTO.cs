namespace LoveShop.DTOs
{
	public record ProductCreateDto(
		string Name,
		string? Description,
		decimal Price,
		ICollection<Guid> CategoriesIDs
	);
}