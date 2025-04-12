using IdentityProvider.API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.API.Data.Seed
{
	public class IdentityDataSeeder
	{
		public static async Task SeedAsync(IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();

			var context = scope.ServiceProvider.GetRequiredService<IdentityProviderDbContext>();

			context.Database.MigrateAsync().GetAwaiter().GetResult();

			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			var roles = new[] { "Admin", "User" };

			// Roles
			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new ApplicationRole { Name = role });
				}
			}

			// Admin
			var adminEmail = "admin@example.com";
			var adminUser = await userManager.FindByEmailAsync(adminEmail);

			if (adminUser == null)
			{
				var admin = new ApplicationUser
				{
					UserName = "admin",
					Email = adminEmail,
					EmailConfirmed = true,
					FirstName = "Admin",
					LastName = "Admin"
				};

				var result = await userManager.CreateAsync(admin, "Admin123!");

				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(admin, "Admin");
				}
				else
				{
					var errors = string.Join(", ", result.Errors.Select(e => e.Description));
					throw new Exception($"Admin creation error: {errors}");
				}
			}
		}
	}
}
