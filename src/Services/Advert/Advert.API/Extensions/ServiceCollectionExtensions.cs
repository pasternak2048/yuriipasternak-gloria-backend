using Advert.API.Data;
using Advert.API.Messaging;
using Advert.API.Models.DTOs.Requests;
using Advert.API.Models.DTOs.Responses;
using Advert.API.Models.Entities;
using Advert.API.Models.Filters;
using Advert.API.Repositories;
using Advert.API.Services;
using BuildingBlocks.Caching;
using BuildingBlocks.Configuration;
using BuildingBlocks.Extensions;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Persistence.Mongo;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Advert.API.Extensions
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
			services.AddSignatureValidation(configuration);
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
			));
			services.AddTransient<DatabaseInitializer<AdvertEntity>>();
			services.AddSingleton<ICollectionSeeder<AdvertEntity>, AdvertSeeder>();
			services.AddSingleton<MongoCollectionSeeder<AdvertEntity>>(provider =>
			{
				var client = provider.GetRequiredService<IMongoClient>();
				var settings = provider.GetRequiredService<MongoSettings>();
				var seeder = provider.GetRequiredService<ICollectionSeeder<AdvertEntity>>();

				return new MongoCollectionSeeder<AdvertEntity>(client, settings, "adverts", seeder);
			});
		}
	}
}
