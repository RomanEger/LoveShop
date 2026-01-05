using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveShop.Models
{
	[Table("products_in_cart")]
	public class ProductInCart : BaseEntity
	{
		[Column("cart_id")] public Guid CartId { get; set; }

		[Column("product_id")] public Guid ProductId { get; set; }

		[Column("quantity")] public int Quantity { get; set; } = 1;

		public Cart Cart { get; set; } = null!;

		public Product Product { get; set; } = null!;
	}

	public class ProductInCartConfiguration : BaseEntityConfiguration<ProductInCart>
	{
		public override void Configure(EntityTypeBuilder<ProductInCart> builder)
		{
			base.Configure(builder);

			builder.Property(pic => pic.Quantity)
				.HasDefaultValue(1);

			builder.HasOne(pic => pic.Cart)
				.WithMany(c => c.ProductsInCart)
				.HasForeignKey(pic => pic.CartId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(pic => pic.Product)
				.WithMany(p => p.ProductInCarts)
				.HasForeignKey(pic => pic.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(pic => new { pic.CartId, pic.ProductId })
				.IsUnique();

			builder.ToTable(t => t.HasCheckConstraint(
				"CK_ProductInCart_Quantity",
				"quantity > 0"));
		}
	}
}