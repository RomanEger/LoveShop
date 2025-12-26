using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace LoveShop.Models
{
    [Table("customers")]
    public partial record Customer : BaseEntity
    {
        [Column("name")]
        public string Name { get; init; } = null!;

        [Column("email")]
        [EmailAddress]
        public string Email { get; init; } = null!;

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

        public ICollection<Cart> Carts { get; init; } = [];

        public ICollection<Order> Orders { get; init; } = [];

        [GeneratedRegex("\\d{11}")]
        private static partial Regex PhoneNumberRegex();
    }

    public class CustomerConfiguration : BaseEntityConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(11);

            builder.HasIndex(c => c.Email)
                .IsUnique();

            builder.HasIndex(c => c.PhoneNumber)
                .IsUnique();
        }
    }
}