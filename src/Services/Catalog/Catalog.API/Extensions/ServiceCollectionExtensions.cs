using Catalog.API.Services.Interfaces;
using Catalog.API.Services;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Repositories;
using System.Reflection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Identity;

namespace Catalog.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCorsPolicy();
			services.AddJwtAuthentication(configuration);
			services.AddMongoInfrastructure(configuration);
			services.AddHttpContextServices();
			services.AddExceptionHandlerServices();
			services.AddSwaggerDocumentation();
			services.AddDistributedCacheServices(configuration);

			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddScoped<IRealtyService, RealtyService>();
			services.AddScoped<RealtyRepository>();
			services.AddScoped<IRealtyRepository>(provider =>
				new CachedRealtyRepository(
					provider.GetRequiredService<RealtyRepository>(),
					provider.GetRequiredService<IDistributedCache>(),
					provider.GetRequiredService<ILogger<CachedRealtyRepository>>()
			));
		}
	}
}
