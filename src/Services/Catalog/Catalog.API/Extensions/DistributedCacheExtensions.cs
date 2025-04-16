namespace Catalog.API.Extensions
{
	public static class DistributedCacheExtensions
	{
		public static void AddDistributedCacheServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = configuration.GetConnectionString("Redis");
			});
		}
	}
}
