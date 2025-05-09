using BuildingBlocks.Extensions;
using BuildingBlocks.Infrastructure;
using DocumentMetadata.API.Models.DTOs.Requests;
using DocumentMetadata.API.Models.DTOs.Responses;
using DocumentMetadata.API.Repositories;
using DocumentMetadata.API.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection;
using System.Text.Json.Serialization;
using DocumentMetadata.API.Models.Filters;
using BuildingBlocks.Caching;
using DocumentMetadata.API.Models.Entities;
using FluentValidation.AspNetCore;
using FluentValidation;

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
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddFluentValidationAutoValidation();
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddHttpContextAccessor();
			services.AddScoped<IGenericService<DocumentMetadataResponse, DocumentMetadataCreateRequest, DocumentMetadataUpdateRequest, DocumentMetadataFilters>, DocumentMetadataService>();
			services.AddScoped<CacheStampManager>();
			services.AddScoped<DocumentMetadataRepository>();
			services.AddScoped<IGenericRepository<DocumentMetadataEntity, DocumentMetadataFilters>>(provider =>
				new CachedGenericRepository<DocumentMetadataEntity, DocumentMetadataFilters>(
					provider.GetRequiredService<DocumentMetadataRepository>(),
					provider.GetRequiredService<IDistributedCache>(),
					provider.GetRequiredService<CacheStampManager>(),
					provider.GetRequiredService<ILogger<CachedGenericRepository<DocumentMetadataEntity, DocumentMetadataFilters>>>()
			));
		}
	}
}
