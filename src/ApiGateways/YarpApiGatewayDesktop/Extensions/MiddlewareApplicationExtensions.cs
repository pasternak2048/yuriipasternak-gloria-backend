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
				options.SwaggerEndpoint("https://localhost:6062/swagger/v1/swagger.json", "Identity Provider API v1");
				options.SwaggerEndpoint("https://localhost:6063/swagger/v1/swagger.json", "Catalog API v1");
				options.SwaggerEndpoint("https://localhost:6064/swagger/v1/swagger.json", "Advert API v1");
				options.SwaggerEndpoint("https://localhost:6069/swagger/v1/swagger.json", "Document Metadata API v1");
				options.SwaggerEndpoint("https://localhost:6070/swagger/v1/swagger.json", "Document Storage API v1");
				options.SwaggerEndpoint("https://localhost:6075/swagger/v1/swagger.json", "Notification API v1");
				options.SwaggerEndpoint("https://localhost:6076/swagger/v1/swagger.json", "Subscription API v1");
				options.DocumentTitle = "API Gateway Swagger";
				options.RoutePrefix = string.Empty;
			});

			app.MapReverseProxy();
		}
	}
}
