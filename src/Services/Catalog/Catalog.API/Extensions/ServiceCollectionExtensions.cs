using BuildingBlocks.Caching;
using BuildingBlocks.Configuration;
using BuildingBlocks.Extensions;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Persistence.Mongo;
using Catalog.API.Data;
using Catalog.API.Models.DTOs.Requests;
using Catalog.API.Models.DTOs.Responses;
using Catalog.API.Models.Entities;
using Catalog.API.Models.Filters;
using Catalog.API.Repositories;
using Catalog.API.Services;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json.Serialization;

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
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddFluentValidationAutoValidation();
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddHttpContextAccessor();
			services.AddScoped<IGenericService<RealtyResponse, RealtyCreateRequest, RealtyUpdateRequest, RealtyFilters>, RealtyService>();
			services.AddScoped<CacheStampManager>();
			services.AddScoped<RealtyRepository>();
			services.AddScoped<IGenericRepository<RealtyEntity, RealtyFilters>>(provider =>
				new CachedGenericRepository<RealtyEntity, RealtyFilters>(
					provider.GetRequiredService<RealtyRepository>(),
					provider.GetRequiredService<IDistributedCache>(),
					provider.GetRequiredService<CacheStampManager>(),
					provider.GetRequiredService<ILogger<CachedGenericRepository<RealtyEntity, RealtyFilters>>>()
			));
			services.AddTransient<DatabaseInitializer<RealtyEntity>>();
			services.AddSingleton<ICollectionSeeder<RealtyEntity>, RealtySeeder>();
			services.AddSingleton<MongoCollectionSeeder<RealtyEntity>>(provider =>
			{
				var client = provider.GetRequiredService<IMongoClient>();
				var settings = provider.GetRequiredService<MongoSettings>();
				var seeder = provider.GetRequiredService<ICollectionSeeder<RealtyEntity>>();

				return new MongoCollectionSeeder<RealtyEntity>(client, settings, "realties", seeder);
			});
		}
	}
}
