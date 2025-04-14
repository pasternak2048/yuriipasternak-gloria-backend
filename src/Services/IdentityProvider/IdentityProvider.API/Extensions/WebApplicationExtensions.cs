using IdentityProvider.API.Data.Seed;
using IdentityProvider.API.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.API.Extensions
{
	public static class WebApplicationExtensions
	{
		public static async Task InitializeDatabaseAsync(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();

			var context = scope.ServiceProvider.GetRequiredService<IdentityProviderDbContext>();
			await context.Database.MigrateAsync();

			await IdentityDataSeeder.SeedAsync(app.Services);
		}

		public static WebApplication UseCustomMiddlewares(this WebApplication app)
		{
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseExceptionHandler(options => { });

			return app;
		}
	}
}
