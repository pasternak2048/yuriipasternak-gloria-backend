using IdentityProvider.API.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.API.Data
{
	public class IdentityProviderDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
	{
		public IdentityProviderDbContext(DbContextOptions<IdentityProviderDbContext> options) : base(options)
		{
		}
	}
}
