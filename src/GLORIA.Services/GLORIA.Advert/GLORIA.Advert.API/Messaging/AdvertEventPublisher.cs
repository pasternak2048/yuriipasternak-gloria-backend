using GLORIA.Contracts.Events;
using MassTransit;

namespace GLORIA.Advert.API.Messaging
{
	public class AdvertEventPublisher : IAdvertEventPublisher
	{
		private readonly IPublishEndpoint _publishEndpoint;

		public AdvertEventPublisher(IPublishEndpoint publishEndpoint)
		{
			_publishEndpoint = publishEndpoint;
		}

		public async Task PublishAdvertCreatedAsync(AdvertCreatedEvent @event)
		{
			await _publishEndpoint.Publish(@event);
		}
	}
}
