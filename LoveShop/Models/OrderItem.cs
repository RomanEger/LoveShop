using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveShop.Models
{
	[Table("order_items")]
	public class OrderItem : BaseEntity
	{
		[Column("order_id")] public Guid OrderId { get; init; }

		[Column("product_id")] public Guid ProductId { get; init; }

		[Column("quantity")] public int Quantity { get; init; } = 1;

		[Column("unit_price")] public decimal UnitPrice { get; init; }

		public Order Order { get; init; } = null!;

		public Product Product { get; init; } = null!;
	}

	public class OrderItemConfiguration : BaseEntityConfiguration<OrderItem>
	{
		public override void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			base.Configure(builder);

			builder.Property(pio => pio.Quantity)
				.HasDefaultValue(1);

			builder.Property(pio => pio.UnitPrice)
				.HasColumnType("decimal(12,2)")
				.IsRequired();

			builder.HasOne(pio => pio.Order)
				.WithMany(o => o.OrderItems)
				.HasForeignKey(pio => pio.OrderId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(pio => pio.Product)
				.WithMany(p => p.OrderItems)
				.HasForeignKey(pio => pio.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(pio => new { pio.OrderId, pio.ProductId })
				.IsUnique();

			builder.ToTable(t => t.HasCheckConstraint(
				"CK_ProductInOrder_Quantity",
				"quantity > 0"));

			builder.ToTable(t => t.HasCheckConstraint(
				"CK_ProductInOrder_UnitPrice",
				"unit_price >= 0"));
		}
	}
}