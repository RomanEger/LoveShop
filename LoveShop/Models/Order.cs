using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveShop.Models
{
	[Table("orders")]
	public class Order : BaseEntity
	{
		[Column("created_at")] public DateTimeOffset CreatedAt { get; init; } = DateTime.UtcNow;

		[Column("completed_at")] public DateTimeOffset? CompletedAt { get; set; }

		[Column("customer_id")] public Guid CustomerId { get; set; }

		[Column("supplier_id")] public Guid SupplierId { get; set; }

		[Column("is_active")] public bool IsActive { get; set; } = true;

		public Customer Customer { get; set; } = null!;

		public Supplier Supplier { get; set; } = null!;

		public ICollection<OrderItem> OrderItems { get; init; } = [];
	}

	public class OrderConfiguration : BaseEntityConfiguration<Order>
	{
		public override void Configure(EntityTypeBuilder<Order> builder)
		{
			base.Configure(builder);

			builder.Property(o => o.IsActive)
				.HasDefaultValue(true);

			builder.HasOne(o => o.Customer)
				.WithMany(c => c.Orders)
				.HasForeignKey(o => o.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(o => o.Supplier)
				.WithMany(s => s.Orders)
				.HasForeignKey(o => o.SupplierId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.ToTable(t => t.HasCheckConstraint(
				"CK_Order_ValidDates",
				"completed_at IS NULL OR created_at <= completed_at"));
		}
	}
}