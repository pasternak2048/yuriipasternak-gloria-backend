using GLORIA.IdentityProvider.API.Models.Configurations;
using GLORIA.IdentityProvider.API.Models.Entities;
using GLORIA.IdentityProvider.API.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GLORIA.IdentityProvider.API.Data
{
	public class IdentityProviderDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
	{
		public IdentityProviderDbContext(DbContextOptions<IdentityProviderDbContext> options) : base(options)
		{
		}

		public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new RefreshTokenConfiguration());
			base.OnModelCreating(builder);
		}
	}
}
