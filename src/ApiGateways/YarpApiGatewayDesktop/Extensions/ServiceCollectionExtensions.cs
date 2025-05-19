using BuildingBlocks.Extensions.Application;

namespace YarpApiGatewayDesktop.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddReverseProxy()
				.LoadFromConfig(configuration.GetSection("ReverseProxy"));
			services.AddCorsPolicy();
		}
	}
}
