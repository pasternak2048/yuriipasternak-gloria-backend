namespace YarpApiGatewayDesktop.Extensions
{
	public static class MiddlewareApplicationExtensions
	{
		public static void UseCustomMiddlewares(this WebApplication app)
		{
			app.UseCors("AllowFromGateway");

			app.UseRouting();

			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("https://localhost:6062/swagger/v1/swagger.json", "IdentityProvider API");
				options.SwaggerEndpoint("https://localhost:6063/swagger/v1/swagger.json", "Catalog API");
				options.SwaggerEndpoint("https://localhost:6069/swagger/v1/swagger.json", "Photo API");
				options.DocumentTitle = "API Gateway Swagger";
				options.RoutePrefix = string.Empty;
			});

			app.MapReverseProxy();
		}
	}
}
