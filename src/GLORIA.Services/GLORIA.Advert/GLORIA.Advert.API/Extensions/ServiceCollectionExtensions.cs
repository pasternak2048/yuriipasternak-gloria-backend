using FluentValidation;
using FluentValidation.AspNetCore;
using GLORIA.Advert.API.Data;
using GLORIA.Advert.API.Messaging;
using GLORIA.Advert.API.Models.Entities;
using GLORIA.Advert.API.Repositories;
using GLORIA.Advert.API.Services;
using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Configuration;
using GLORIA.BuildingBlocks.Extensions.Application;
using GLORIA.BuildingBlocks.Extensions.Infrastructure;
using GLORIA.BuildingBlocks.Infrastructure.Data.Caching;
using GLORIA.BuildingBlocks.Infrastructure.Data.Mongo;
using GLORIA.BuildingBlocks.Infrastructure.Data.Repositories;
using GLORIA.Contracts.Dtos.Advert;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json.Serialization;

namespace GLORIA.Advert.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCurrentUser();
			services.AddJwtAuthentication(configuration);
			services.AddCorsPolicy();
			services.AddExceptionHandlerServices();
			services.AddSwaggerDocumentation("Advert API");
			services.AddMongoInfrastructure(configuration);
			services.AddDistributedCache(configuration);
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddFluentValidationAutoValidation();
			services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
			services.AddMassTransit(x =>
			{
				x.UsingRabbitMq((context, cfg) =>
				{
					var settings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

					cfg.Host(settings.Host, settings.VirtualHost, h =>
					{
						h.Username(settings.Username);
						h.Password(settings.Password);
					});
				});
			});
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddHttpContextAccessor();
			services.AddScoped<IAdvertEventPublisher, AdvertEventPublisher>();
			services.AddScoped<IGenericService<AdvertResponse, AdvertCreateRequest, AdvertUpdateRequest, AdvertFilters>, AdvertService>();
			services.AddScoped<CacheStampManager>();
			services.AddScoped<AdvertRepository>();
			services.AddScoped<IGenericRepository<AdvertEntity, AdvertFilters>>(provider =>
				new CachedGenericRepository<AdvertEntity, AdvertFilters>(
					provider.GetRequiredService<AdvertRepository>(),
					provider.GetRequiredService<IDistributedCache>(),
					provider.GetRequiredService<CacheStampManager>(),
					provider.GetRequiredService<ILogger<CachedGenericRepository<AdvertEntity, AdvertFilters>>>()
				)
			);
			services.AddTransient<DatabaseInitializer<AdvertEntity>>();
			services.AddSingleton<ICollectionSeeder<AdvertEntity>, AdvertSeeder>();
			services.AddSingleton(provider =>
			{
				var client = provider.GetRequiredService<IMongoClient>();
				var settings = provider.GetRequiredService<MongoSettings>();
				var seeder = provider.GetRequiredService<ICollectionSeeder<AdvertEntity>>();

				return new MongoCollectionSeeder<AdvertEntity>(client, settings, "adverts", seeder);
			});
		}
	}
}
