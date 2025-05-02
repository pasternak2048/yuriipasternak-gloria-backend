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
				options.SwaggerEndpoint("https://localhost:6064/swagger/v1/swagger.json", "Advert API");
				options.SwaggerEndpoint("https://localhost:6069/swagger/v1/swagger.json", "Document Metadata API");
				options.SwaggerEndpoint("https://localhost:6070/swagger/v1/swagger.json", "Document Storage API");
				options.DocumentTitle = "API Gateway Swagger";
				options.RoutePrefix = string.Empty;
			});

			app.MapReverseProxy();
		}
	}
}
