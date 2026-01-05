using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveShop.Models
{
	[Table("carts")]
	public class Cart : BaseEntity
	{
		[Column("customer_id")] public Guid CustomerId { get; set; }

		public Customer Customer { get; set; } = null!;

		public ICollection<ProductInCart> ProductsInCart { get; init; } = [];
	}

	public class CartConfiguration : BaseEntityConfiguration<Cart>
	{
		public override void Configure(EntityTypeBuilder<Cart> builder)
		{
			base.Configure(builder);

			builder.HasOne(c => c.Customer)
				.WithMany(c => c.Carts)
				.HasForeignKey(c => c.CustomerId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}