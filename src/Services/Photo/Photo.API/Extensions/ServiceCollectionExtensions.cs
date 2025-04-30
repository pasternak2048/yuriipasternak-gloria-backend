using BuildingBlocks.Extensions;
using BuildingBlocks.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Photo.API.Models;
using Photo.API.Models.DTOs.Requests;
using Photo.API.Models.DTOs.Responses;
using Photo.API.Repositories;
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
			services.AddScoped<IGenericService<RealtyPhotoMetadataResponse, CreateRealtyPhotoMetadataRequest, UpdateRealtyPhotoMetadataRequest, RealtyPhotoFilters>, RealtyPhotoService>();
			services.AddScoped<IFileStorageService, FileStorageService>();
			services.AddSingleton<IFileValidatorService, FileValidatorService>();
			services.AddScoped<RealtyPhotoRepository>();
			services.AddScoped<IGenericRepository<RealtyPhotoMetadata, RealtyPhotoFilters>>(provider =>
				new CachedGenericRepository<RealtyPhotoMetadata, RealtyPhotoFilters>(
					provider.GetRequiredService<RealtyPhotoRepository>(),
					provider.GetRequiredService<IDistributedCache>(),
					provider.GetRequiredService<ILogger<CachedGenericRepository<RealtyPhotoMetadata, RealtyPhotoFilters>>>()
			));
		}
	}
}
