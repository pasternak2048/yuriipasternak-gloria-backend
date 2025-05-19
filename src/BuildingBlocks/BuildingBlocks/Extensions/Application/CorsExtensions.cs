using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Extensions.Application
{
	public static class CorsExtensions
	{
		public static void AddCorsPolicy(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(policy =>
					policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

				options.AddPolicy("AllowFromGateway", policy =>
					policy.WithOrigins("https://localhost:6061")
						  .AllowAnyHeader()
						  .AllowAnyMethod()
						  .AllowCredentials());
			});
		}
	}
}
