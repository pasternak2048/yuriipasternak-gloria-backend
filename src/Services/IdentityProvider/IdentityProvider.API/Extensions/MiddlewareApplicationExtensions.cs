using IdentityProvider.API.Data.Seed;
using IdentityProvider.API.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.API.Extensions
{
	public static class MiddlewareApplicationExtensions
	{
		public async static Task UseCustomMiddlewares(this WebApplication app)
		{
			app.UseRouting();
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
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Provider API v1");
					options.RoutePrefix = string.Empty;
				});

				using var scope = app.Services.CreateScope();
				var context = scope.ServiceProvider.GetRequiredService<IdentityProviderDbContext>();
				await context.Database.MigrateAsync();
				await IdentityDataSeeder.SeedAsync(app.Services);
			}
		}
	}
}
