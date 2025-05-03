using Contracts.Events;

namespace Advert.API.Messaging
{
	public interface IAdvertEventPublisher
	{
		Task PublishAdvertCreatedAsync(AdvertCreatedEvent @event);
	}
}
