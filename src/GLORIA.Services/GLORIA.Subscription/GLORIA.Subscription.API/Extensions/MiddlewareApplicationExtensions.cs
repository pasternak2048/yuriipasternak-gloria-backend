namespace GLORIA.Subscription.API.Extensions
{
	public static class MiddlewareApplicationExtensions
	{
		public static void UseCustomMiddlewares(this WebApplication app)
		{
			app.UseExceptionHandler(_ => { });
			app.UseCors("AllowFromGateway");
			app.MapControllers();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "Subscription API v1");
					options.RoutePrefix = string.Empty;
				});
			}
		}
	}
}
