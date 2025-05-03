using BuildingBlocks.Configuration;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Extensions
{
	public static class RabbitMqExtensions
	{
		public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
		{
			var settings = configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();

			services.AddMassTransit(x =>
			{
				x.UsingRabbitMq((context, cfg) =>
				{
					cfg.Host(settings.Host, settings.VirtualHost, h =>
					{
						h.Username(settings.Username);
						h.Password(settings.Password);
					});
				});
			});
		}
	}
}
