using BuildingBlocks.Extensions;
using BuildingBlocks.Infrastructure;
using Catalog.API.Data;
using Catalog.API.Models;
using Catalog.API.Models.DTOs.Requests;
using Catalog.API.Models.DTOs.Responses;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection;

namespace Catalog.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCurrentUser();
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
			services.AddScoped<IGenericService<RealtyResponse, RealtyCreateRequest, RealtyUpdateRequest, RealtyFilters>, RealtyService>();
			services.AddScoped<RealtyRepository>();
			services.AddScoped<IGenericRepository<Realty, RealtyFilters>>(provider =>
				new CachedGenericRepository<Realty, RealtyFilters>(
					provider.GetRequiredService<RealtyRepository>(),
					provider.GetRequiredService<IDistributedCache>(),
					provider.GetRequiredService<ILogger<CachedGenericRepository<Realty, RealtyFilters>>>()
			));
			services.AddTransient<RealtyDataSeeder>();
			services.AddTransient<DatabaseInitializer>();
		}
	}
}
