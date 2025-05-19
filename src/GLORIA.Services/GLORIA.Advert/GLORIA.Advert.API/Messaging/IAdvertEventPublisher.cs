using GLORIA.Contracts.Events;

namespace GLORIA.Advert.API.Messaging
{
	public interface IAdvertEventPublisher
	{
		Task PublishAdvertCreatedAsync(AdvertCreatedEvent @event);
	}
}
