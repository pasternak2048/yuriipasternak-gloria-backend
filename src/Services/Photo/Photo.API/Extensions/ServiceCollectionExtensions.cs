using BuildingBlocks.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Photo.API.Repositories;
using Photo.API.Repositories.Interfaces;
using Photo.API.Services;
using Photo.API.Services.Interfaces;
using System.Reflection;

namespace Photo.API.Extensions
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
			services.AddControllers();
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddHttpContextAccessor();
			services.AddScoped<IRealtyPhotoService, RealtyPhotoService>();
			services.AddScoped<IFileStorageService, FileStorageService>();
			services.AddSingleton<IFileValidatorService, FileValidatorService>();
			services.AddScoped<RealtyPhotoRepository>();
			services.AddScoped<IRealtyPhotoRepository>(provider =>
				new CachedRealtyPhotoRepository(
					provider.GetRequiredService<RealtyPhotoRepository>(),
					provider.GetRequiredService<IDistributedCache>(),
					provider.GetRequiredService<ILogger<CachedRealtyPhotoRepository>>()
			));
		}
	}
}
