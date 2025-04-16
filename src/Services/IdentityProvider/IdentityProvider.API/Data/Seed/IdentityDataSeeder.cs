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
					Id = Guid.Parse("2d5c1afc-a67f-4a26-b807-56c2fd43d10d"),
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

			// User
			var userEmail = "pasternak2048@example.com";
			var userUser = await userManager.FindByEmailAsync(userEmail);

			if (userUser == null)
			{
				var user = new ApplicationUser
				{
					Id = Guid.Parse("1fef01ff-3306-4d4f-a69d-6b4776142ecd"),
					UserName = "pasternak2048",
					Email = userEmail,
					EmailConfirmed = true,
					FirstName = "Yurii",
					LastName = "Pasternak"
				};

				var result = await userManager.CreateAsync(user, "11111111_Aa!");

				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, "User");
				}
				else
				{
					var errors = string.Join(", ", result.Errors.Select(e => e.Description));
					throw new Exception($"User creation error: {errors}");
				}
			}
		}
	}
}
