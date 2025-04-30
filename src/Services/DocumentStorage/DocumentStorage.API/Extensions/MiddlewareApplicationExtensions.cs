namespace DocumentStorage.API.Extensions
{
	public static class MiddlewareApplicationExtensions
	{
		public static void UseCustomMiddlewares(this WebApplication app)
		{
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseExceptionHandler(_ => { });
			app.UseCors("AllowFromGateway");
			app.MapControllers();
			app.UseStaticFiles();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "Document Storage API v1");
					options.RoutePrefix = string.Empty;
				});
			}
		}
	}
}