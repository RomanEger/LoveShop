namespace LoveShop.DTOs.Category
{
	public sealed record CategoryUpdateDTO(
		Guid Id,
		string Name,
		Guid? ParentCategoryId);
}