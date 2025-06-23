using FluentValidation;
using FluentValidation.AspNetCore;
using GLORIA.BuildingBlocks.Configuration;
using GLORIA.BuildingBlocks.Extensions.Application;
using GLORIA.BuildingBlocks.Extensions.Infrastructure;
using GLORIA.Notification.API.Events.Consumers.Advert;
using GLORIA.Notification.API.ExternalServices.Subscription;
using GLORIA.Notification.API.Repositories;
using GLORIA.Notification.API.Services;
using GLORIA.Notification.API.Services.Interfaces;
using LYRA.Client.Extensions;
using LYRA.Client.Interfaces;
using LYRA.Client.Models;
using LYRA.Client.Signers.Http;
using LYRA.Security.Enums;
using MassTransit;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Text.Json.Serialization;

namespace GLORIA.Notification.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCurrentUser();
			services.AddJwtAuthentication(configuration);
			services.AddCorsPolicy();
			services.AddExceptionHandlerServices();
			services.AddSwaggerDocumentation("Notification API");
			services.AddMongoInfrastructure(configuration);
			services.AddDistributedCache(configuration);
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddFluentValidationAutoValidation();
			services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
            services.AddHttpClient<ILyraSignedHttpClient, LyraSignedHttpClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Services:Subscription"]);
            });
            services.AddScoped<SubscriptionClient>();
			services.AddMassTransit(x =>
			{
				x.AddConsumer<AdvertCreatedEventConsumer>();

				x.UsingRabbitMq((context, cfg) =>
				{
					var settings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

					cfg.Host(settings.Host, settings.VirtualHost, h =>
					{
						h.Username(settings.Username);
						h.Password(settings.Password);
					});

					cfg.ReceiveEndpoint("advert-created-event", e =>
					{
						e.ConfigureConsumer<AdvertCreatedEventConsumer>(context);
					});
				});
			});
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddHttpContextAccessor();
			services.AddScoped<INotificationRepository, NotificationRepository>();
			services.AddScoped<INotificationService, NotificationService>();

			services.AddLyraAsCaller(opts =>
			{
                opts.Touchpoints = new List<LyraTouchpoint>
				{
					new()
					{
						SystemName = "notification@gloria",
						Secret = "super-secret-notification-key",
						Context = AccessContext.Http,
						SignatureType = SignatureType.HMAC
					}
				};
            });
		}
	}
}
