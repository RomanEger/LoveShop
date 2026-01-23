using LoveShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoveShop.Persistence
{
	public class IdentityDbContext : IdentityDbContext<User, Role, Guid>
	{
		public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.HasDefaultSchema("identity");
		}
	}
}