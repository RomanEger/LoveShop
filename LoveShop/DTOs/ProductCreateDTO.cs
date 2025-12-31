using LoveShop.Models;

namespace LoveShop.DTOs
{
	public record ProductCreateDto(
		string Name,
		string? Description,
		decimal Price,
		ICollection<Guid> ProductCategoriesIDs)
	{
		public Product ToProduct()
		{
			return new Product
			{
				Name = Name, Description = Description, Price = Price, ProductCategoriesIDs = ProductCategoriesIDs
			};
		}
	}
}