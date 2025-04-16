namespace YarpApiGatewayDesktop.Extensions
{
	public static class CorsExtensions
	{
		public static void AddCorsPolicy(this IServiceCollection services)
		{
			services.AddCors(opt =>
			{
				opt.AddPolicy("AllowSwaggerFromGateway", policy =>
				{
					policy.WithOrigins("https://localhost:6061")
						  .AllowAnyMethod()
						  .AllowAnyHeader();
				});
			});
		}
	}
}
