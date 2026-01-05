using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveShop.Models
{
	[Table("product_categories")]
	public class ProductCategory : BaseEntity
	{
		[Column("product_id")] public Guid ProductId { get; set; }

		[Column("category_id")] public Guid CategoryId { get; set; }

		public Product Product { get; set; } = null!;

		public Category Category { get; set; } = null!;
	}

	public class ProductCategoryConfiguration : BaseEntityConfiguration<ProductCategory>
	{
		public override void Configure(EntityTypeBuilder<ProductCategory> builder)
		{
			base.Configure(builder);

			builder.HasOne(pc => pc.Product)
				.WithMany(p => p.ProductCategories)
				.HasForeignKey(pc => pc.ProductId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(pc => pc.Category)
				.WithMany(c => c.ProductCategories)
				.HasForeignKey(pc => pc.CategoryId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasIndex(pc => new { pc.ProductId, pc.CategoryId })
				.IsUnique();
		}
	}
}