using LoveShop.DTOs.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveShop.Models
{
	[Table("products")]
	public class Product : BaseEntity
	{
		private Product(
			string name,
			string description,
			decimal price,
			ICollection<ProductCategory>? productCategories = null,
			ICollection<ProductInCart>? productInCarts = null,
			ICollection<OrderItem>? orderItems = null)
		{
			Name = name;
			Description = description;
			Price = price;
			ProductCategories = productCategories ?? [];
			ProductInCarts = productInCarts ?? [];
			OrderItems = orderItems ?? [];
		}

		public static Product Create(
			string name,
			string description,
			decimal price,
			ICollection<ProductCategory>? productCategories = null)
		{
			return new Product(name, description, price, productCategories);
		}

		[Column("name")] public string Name { get; set; }

		[Column("description")] public string? Description { get; set; }

		[Column("price")] public decimal Price { get; set; }

		public ICollection<ProductCategory> ProductCategories { get; init; }

		public ICollection<ProductInCart> ProductInCarts { get; init; }

		public ICollection<OrderItem> OrderItems { get; init; }

		public ProductDTO ToDTO()
		{
			return new ProductDTO(
				Id,
				Name,
				Description ?? string.Empty,
				Price,
				ProductCategories.Select(pc => pc.CategoryId).ToArray(),
				RowVersion);
		}
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
				.HasMaxLength(1000);

			builder.Property(p => p.Price)
				.HasColumnType("decimal(12,2)")
				.IsRequired();

			builder.ToTable(t => t.HasCheckConstraint(
				"CK_Product_Price",
				"price >= 0"));
		}
	}
}