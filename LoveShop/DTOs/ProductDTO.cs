namespace LoveShop.DTOs
{
	public sealed record ProductDTO(
		Guid Id,
		string Name,
		string Description,
		decimal Price,
		Guid CategoryId,
		byte[] RowVersion);
}