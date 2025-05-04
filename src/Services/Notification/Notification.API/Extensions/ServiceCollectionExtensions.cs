using BuildingBlocks.Configuration;
using BuildingBlocks.Extensions;
using MassTransit;
using Microsoft.Extensions.Options;
using Notification.API.Events.Consumers.Advert;
using Notification.API.Repositories;
using Notification.API.Services;
using Notification.API.Services.Interfaces;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Notification.API.Extensions
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
			services.AddSignatureValidation(configuration);
			//services.AddMassTransit(x =>
			//{
			//	x.AddConsumer<AdvertCreatedEventConsumer>();

			//	x.UsingRabbitMq((context, cfg) =>
			//	{
			//		var settings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

			//		cfg.Host(settings.Host, settings.VirtualHost, h =>
			//		{
			//			h.Username(settings.Username);
			//			h.Password(settings.Password);
			//		});

			//		cfg.ReceiveEndpoint("advert-created-event", e =>
			//		{
			//			e.ConfigureConsumer<AdvertCreatedEventConsumer>(context);
			//		});
			//	});
			//});
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddHttpContextAccessor();
			services.AddScoped<INotificationRepository, NotificationRepository>();
			services.AddScoped<INotificationService, NotificationService>();
		}
	}
}
