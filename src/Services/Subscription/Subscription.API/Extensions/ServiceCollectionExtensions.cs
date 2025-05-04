using BuildingBlocks.Extensions;
using BuildingBlocks.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Subscription.API.Models.DTOs.Requests;
using Subscription.API.Models.DTOs.Responses;
using Subscription.API.Models.Entities;
using Subscription.API.Models.Filters;
using Subscription.API.Repositories;
using Subscription.API.Services;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Subscription.API.Extensions
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
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddHttpContextAccessor();
			services.AddScoped<IGenericService<SubscriptionResponse, SubscriptionCreateRequest, SubscriptionUpdateRequest, SubscriptionFilters>, SubscriptionService>();
			services.AddScoped<SubscriptionRepository>();
			services.AddScoped<IGenericRepository<SubscriptionEntity, SubscriptionFilters>>(provider =>
				new CachedGenericRepository<SubscriptionEntity, SubscriptionFilters>(
					provider.GetRequiredService<SubscriptionRepository>(),
					provider.GetRequiredService<IDistributedCache>(),
					provider.GetRequiredService<ILogger<CachedGenericRepository<SubscriptionEntity, SubscriptionFilters>>>()
			));
		}
	}
}
