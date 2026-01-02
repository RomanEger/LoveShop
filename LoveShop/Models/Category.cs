using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveShop.Models
{
	[Table("categories")]
	public class Category : BaseEntity
	{
		[Column("name")] public string Name { get; init; } = null!;

		[Column("parent_category_id")]
		public Guid? ParentCategoryId
		{
			get;
			init
			{
				if (value == Id)
				{
					throw new ArgumentException("Circular reference");
				}

				field = value;
			}
		}

		public Category? ParentCategory
		{
			get;
			init
			{
				if (value == this)
				{
					throw new ArgumentException("Circular reference");
				}

				field = value;
			}
		}

		public ICollection<Category> ChildCategories { get; init; } = [];

		public ICollection<ProductCategory> ProductCategories { get; set; } = [];
	}

	public class CategoryConfiguration : BaseEntityConfiguration<Category>
	{
		public override void Configure(EntityTypeBuilder<Category> builder)
		{
			base.Configure(builder);

			builder.Property(c => c.Name)
				.IsRequired()
				.HasMaxLength(200);

			builder.HasOne(c => c.ParentCategory)
				.WithMany(c => c.ChildCategories)
				.HasForeignKey(c => c.ParentCategoryId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.ToTable(t => t.HasCheckConstraint(
				"CK_Category_SelfReference",
				"id != parent_category_id"));
		}
	}
}