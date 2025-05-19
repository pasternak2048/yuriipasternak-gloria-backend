namespace GLORIA.YarpApiGatewayDesktop.Extensions
{
	public static class MiddlewareApplicationExtensions
	{
		public static void UseCustomMiddlewares(this WebApplication app)
		{
			app.UseCors("AllowFromGateway");

			app.UseRouting();

			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/identity/swagger/v1/swagger.json", "Identity Provider API v1");
				options.SwaggerEndpoint("/catalog/swagger/v1/swagger.json", "Catalog API v1");
				options.SwaggerEndpoint("/advert/swagger/v1/swagger.json", "Advert API v1");
				options.SwaggerEndpoint("/documentmetadata/swagger/v1/swagger.json", "Document Metadata API v1");
				options.SwaggerEndpoint("/documentstorage/swagger/v1/swagger.json", "Document Storage API v1");
				options.SwaggerEndpoint("/notification/swagger/v1/swagger.json", "Notification API v1");
				options.SwaggerEndpoint("/subscription/swagger/v1/swagger.json", "Subscription API v1");
				options.DocumentTitle = "API Gateway Swagger";
				options.RoutePrefix = string.Empty;
			});

			app.MapReverseProxy();
		}
	}
}
