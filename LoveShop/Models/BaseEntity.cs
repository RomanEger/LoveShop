using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveShop.Models
{
	public abstract class BaseEntity
	{
		protected BaseEntity()
		{
			RowVersion = new byte[20];
		}

		[Column("id")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; init; }

		[Column("is_deleted")] public bool IsDeleted { get; set; } = false;

		[Column("row_version")]
		public byte[] RowVersion { get; set; } = null!;
	}

	public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
	{
		public virtual void Configure(EntityTypeBuilder<T> builder)
		{
			builder.HasKey(e => e.Id);

			builder.Property(e => e.IsDeleted).HasDefaultValue(false);

			builder.Property(p => p.RowVersion).IsRowVersion();
		}
	}
}