using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GLORIA.BuildingBlocks.Extensions.Infrastructure
{
	public static class DistributedCacheExtensions
	{
		public static void AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = configuration.GetConnectionString("Redis");
			});
		}
	}
}
