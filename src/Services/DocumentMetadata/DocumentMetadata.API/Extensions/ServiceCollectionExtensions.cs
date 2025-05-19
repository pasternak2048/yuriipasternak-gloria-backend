using BuildingBlocks.Caching;
using BuildingBlocks.Extensions;
using BuildingBlocks.Infrastructure;
using Contracts.Dtos.DocumentMetadata;
using DocumentMetadata.API.Models.Entities;
using DocumentMetadata.API.Repositories;
using DocumentMetadata.API.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection;
using System.Text.Json.Serialization;

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
