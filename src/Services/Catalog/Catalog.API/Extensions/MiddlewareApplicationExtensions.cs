using Catalog.API.Services;

namespace Catalog.API.Extensions
{
	public static class MiddlewareApplicationExtensions
	{
		public async static Task UseCustomMiddlewares(this WebApplication app)
		{
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseExceptionHandler(_ => { });
			app.UseCors("AllowFromGateway");
			app.MapControllers();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1");
					options.RoutePrefix = string.Empty;
				});

				using var scope = app.Services.CreateScope();
				var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
				await initializer.InitializeAsync();
			}
		}
	}
}
