﻿using GLORIA.Advert.API.Models.Entities;
using GLORIA.BuildingBlocks.Infrastructure.Data.Mongo;

namespace GLORIA.Advert.API.Extensions
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
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "Advert API v1");
					options.RoutePrefix = string.Empty;
				});

				using var scope = app.Services.CreateScope();
				var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer<AdvertEntity>>();
				await initializer.InitializeAsync();
			}
		}
	}
}
