using IdentityProvider.API.Data;
using IdentityProvider.API.Models.Identity;
using IdentityProvider.API.Services.Interfaces;
using IdentityProvider.API.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.API.Extensions
{
	public static class IdentityExtensions
	{
		public static void AddIdentityCoreServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<ITokenService, TokenService>();

			services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
				options.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<IdentityProviderDbContext>()
			.AddDefaultTokenProviders();
		}
	}
}
