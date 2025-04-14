using Catalog.API.Services;

namespace Catalog.API.Extensions
{
	public static class WebApplicationExtensions
	{
		public static async Task InitializeDatabaseAsync(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();

			var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
			await initializer.InitializeAsync();
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
