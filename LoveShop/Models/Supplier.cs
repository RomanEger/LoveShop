using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace LoveShop.Models
{
	[Table("suppliers")]
	public partial class Supplier : BaseEntity
	{
		[Column("name")] public string Name { get; init; } = null!;

		[Column("email")][EmailAddress] public string Email { get; init; } = null!;

		[Column("phone_number")]
		public string PhoneNumber
		{
			get;
			init
			{
				var regex = PhoneNumberRegex();
				if (!regex.IsMatch(value))
				{
					throw new ArgumentException("Incorrect phone number");
				}

				field = value;
			}
		} = null!;

		public ICollection<Order> Orders { get; init; } = [];

		[GeneratedRegex("\\d{11}")]
		private static partial Regex PhoneNumberRegex();
	}

	public class SupplierConfiguration : BaseEntityConfiguration<Supplier>
	{
		public override void Configure(EntityTypeBuilder<Supplier> builder)
		{
			base.Configure(builder);

			builder.Property(s => s.Name)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(s => s.Email)
				.IsRequired()
				.HasMaxLength(255);

			builder.Property(s => s.PhoneNumber)
				.IsRequired()
				.HasMaxLength(11);

			builder.HasIndex(s => s.Email)
				.IsUnique();

			builder.HasIndex(s => s.PhoneNumber)
				.IsUnique();
		}
	}
}