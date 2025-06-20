using FluentValidation;
using FluentValidation.AspNetCore;
using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Configuration;
using GLORIA.BuildingBlocks.Extensions.Application;
using GLORIA.BuildingBlocks.Extensions.Infrastructure;
using GLORIA.BuildingBlocks.Infrastructure.Data.Caching;
using GLORIA.BuildingBlocks.Infrastructure.Data.Mongo;
using GLORIA.BuildingBlocks.Infrastructure.Data.Repositories;
using GLORIA.Catalog.API.Data;
using GLORIA.Catalog.API.Models.Entities;
using GLORIA.Catalog.API.Repositories;
using GLORIA.Catalog.API.Services;
using GLORIA.Contracts.Dtos.Catalog;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json.Serialization;

namespace GLORIA.Catalog.API.Extensions
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
			services.AddSingleton(provider =>
			{
				var client = provider.GetRequiredService<IMongoClient>();
				var settings = provider.GetRequiredService<MongoSettings>();
				var seeder = provider.GetRequiredService<ICollectionSeeder<RealtyEntity>>();

				return new MongoCollectionSeeder<RealtyEntity>(client, settings, "realties", seeder);
			});
		}
	}
}
