using Contracts.Events;
using MassTransit;

namespace Notification.API.Events.Consumers.Advert
{
	public class AdvertCreatedEventConsumer : IConsumer<AdvertCreatedEvent>
	{
		private readonly ILogger<AdvertCreatedEventConsumer> _logger;

		public AdvertCreatedEventConsumer(ILogger<AdvertCreatedEventConsumer> logger)
		{
			_logger = logger;
		}

		public Task Consume(ConsumeContext<AdvertCreatedEvent> context)
		{
			var message = context.Message;

			_logger.LogInformation("Received AdvertCreatedEvent: {Title}, {City}, {Price}{Currency}",
				message.Title, message.City, message.Price, message.Currency);

			return Task.CompletedTask;
		}
	}
}
