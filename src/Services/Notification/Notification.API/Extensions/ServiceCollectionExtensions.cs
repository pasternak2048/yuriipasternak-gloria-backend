using BuildingBlocks.Configuration;
using BuildingBlocks.Extensions;
using MassTransit;
using Microsoft.Extensions.Options;
using Notification.API.Events.Consumers.Advert;

namespace Notification.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCorsPolicy();
			services.AddExceptionHandlerServices();
			services.AddSwaggerDocumentation("Notification API");
			services.AddControllers();
			services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
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
		}
	}
}
