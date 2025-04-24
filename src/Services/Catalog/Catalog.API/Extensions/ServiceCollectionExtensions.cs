using BuildingBlocks.Extensions;
using Catalog.API.Data;
using Catalog.API.Repositories;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services;
using Catalog.API.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection;

namespace Catalog.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddJwtAuthentication(configuration);
			services.AddCorsPolicy();
			services.AddExceptionHandlerServices();
			services.AddSwaggerDocumentation("Catalog API");
			services.AddMongoInfrastructure(configuration);
			services.AddDistributedCache(configuration);
			services.AddSignatureValidation(configuration);
			services.AddControllers();
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddHttpContextAccessor();
			services.AddScoped<IUserContextService, UserContextService>();
			services.AddScoped<IRealtyService, RealtyService>();
			services.AddScoped<RealtyRepository>();
			services.AddScoped<IRealtyRepository>(provider =>
				new CachedRealtyRepository(
					provider.GetRequiredService<RealtyRepository>(),
					provider.GetRequiredService<IDistributedCache>(),
					provider.GetRequiredService<ILogger<CachedRealtyRepository>>()
			));
			services.AddTransient<RealtyDataSeeder>();
			services.AddTransient<DatabaseInitializer>();
		}
	}
}
