using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveShop.Models
{
	[Table("products")]
	public record Product : BaseEntity
	{
		[Column("name")] public string Name { get; init; } = null!;

		[Column("description")] public string? Description { get; init; }

		[Column("price")]
		public decimal Price
		{
			get;
			init;
		}

		[NotMapped] public ICollection<Guid> ProductCategoriesIDs { get; init; } = [];

		[NotMapped] public ICollection<Guid> ProductInCartsIDs { get; init; } = [];

		[NotMapped] public ICollection<Guid> OrderItemsIDs { get; init; } = [];

		public ICollection<ProductCategory> ProductCategories { get; init; } = [];

		public ICollection<ProductInCart> ProductInCarts { get; init; } = [];

		public ICollection<OrderItem> OrderItems { get; init; } = [];
	}

	public class ProductConfiguration : BaseEntityConfiguration<Product>
	{
		public override void Configure(EntityTypeBuilder<Product> builder)
		{
			base.Configure(builder);

			builder.Property(p => p.Name)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(p => p.Description)
				.HasColumnType("text");

			builder.Property(p => p.Price)
				.HasColumnType("decimal(12,2)")
				.IsRequired();

			builder.ToTable(t => t.HasCheckConstraint(
				"CK_Product_Price",
				"price >= 0"));
		}
	}
}