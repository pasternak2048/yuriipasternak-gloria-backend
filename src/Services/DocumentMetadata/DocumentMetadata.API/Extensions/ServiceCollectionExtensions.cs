using BuildingBlocks.Extensions;
using BuildingBlocks.Infrastructure;
using DocumentMetadata.API.Models.DTOs.Requests;
using DocumentMetadata.API.Models.DTOs.Responses;
using DocumentMetadata.API.Models;
using DocumentMetadata.API.Repositories;
using DocumentMetadata.API.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection;
using DocumentMetadataEntity = DocumentMetadata.API.Models.DocumentMetadata;

namespace DocumentMetadata.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCurrentUser();
			services.AddJwtAuthentication(configuration);
			services.AddCorsPolicy();
			services.AddExceptionHandlerServices();
			services.AddSwaggerDocumentation("Document Metadata API");
			services.AddMongoInfrastructure(configuration);
			services.AddDistributedCache(configuration);
			services.AddSignatureValidation(configuration);
			services.AddControllers();
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddHttpContextAccessor();
			services.AddScoped<IGenericService<DocumentMetadataResponse, DocumentMetadataCreateRequest, DocumentMetadataUpdateRequest, DocumentMetadataFilters>, DocumentMetadataService>();
			services.AddScoped<DocumentMetadataRepository>();
			services.AddScoped<IGenericRepository<DocumentMetadataEntity, DocumentMetadataFilters>>(provider =>
				new CachedGenericRepository<DocumentMetadataEntity, DocumentMetadataFilters>(
					provider.GetRequiredService<DocumentMetadataRepository>(),
					provider.GetRequiredService<IDistributedCache>(),
					provider.GetRequiredService<ILogger<CachedGenericRepository<DocumentMetadataEntity, DocumentMetadataFilters>>>()
			));
		}
	}
}
