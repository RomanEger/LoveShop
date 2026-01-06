namespace LoveShop.DTOs
{
	public record ProductCreateDTO(
		string Name,
		string? Description,
		decimal Price,
		ICollection<Guid> CategoriesIDs);
}