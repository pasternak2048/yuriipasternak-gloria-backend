using FluentValidation;
using FluentValidation.AspNetCore;
using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Extensions.Application;
using GLORIA.BuildingBlocks.Extensions.Infrastructure;
using GLORIA.BuildingBlocks.Infrastructure.Data.Caching;
using GLORIA.BuildingBlocks.Infrastructure.Data.Repositories;
using GLORIA.Contracts.Dtos.Subscription;
using GLORIA.Subscription.API.Models.Entities;
using GLORIA.Subscription.API.Repositories;
using GLORIA.Subscription.API.Repositories.Interfaces;
using GLORIA.Subscription.API.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Reflection;
using System.Text.Json.Serialization;

namespace GLORIA.Subscription.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCurrentUser();
			services.AddJwtAuthentication(configuration);
			services.AddCorsPolicy();
			services.AddExceptionHandlerServices();
			services.AddSwaggerDocumentation("Subscription API");
			services.AddMongoInfrastructure(configuration);
			services.AddDistributedCache(configuration);
			services.AddSignatureValidation(configuration);
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddFluentValidationAutoValidation();
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddHttpContextAccessor();
			services.AddScoped<IGenericService<AdvertSubscriptionResponse, AdvertSubscriptionCreateRequest, AdvertSubscriptionUpdateRequest, AdvertSubscriptionFilters>, AdvertSubscriptionService>();
			services.AddScoped<CacheStampManager>();
			services.AddScoped<AdvertSubscriptionRepository>();
			services.AddScoped<IGenericRepository<AdvertSubscriptionEntity, AdvertSubscriptionFilters>>(provider =>
				new CachedGenericRepository<AdvertSubscriptionEntity, AdvertSubscriptionFilters>(
					provider.GetRequiredService<AdvertSubscriptionRepository>(),
					provider.GetRequiredService<IDistributedCache>(),
					provider.GetRequiredService<CacheStampManager>(),
					provider.GetRequiredService<ILogger<CachedGenericRepository<AdvertSubscriptionEntity, AdvertSubscriptionFilters>>>()
			));
			services.AddScoped<IAdvertSubscriptionLookupRepository, AdvertSubscriptionLookupRepository>();
			services.AddScoped<AdvertSubscriptionMatchingService>();
		}
	}
}
