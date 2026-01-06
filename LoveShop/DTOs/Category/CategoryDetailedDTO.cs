namespace LoveShop.DTOs.Category
{
	public sealed record CategoryDetailedDTO(
		Guid Id,
		string Name,
		Guid? ParentCategoryId,
		ICollection<Guid> ChildCategoriesIds,
		ICollection<Guid> ProductsIds);
}