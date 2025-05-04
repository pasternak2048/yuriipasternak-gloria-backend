using Contracts.Events;
using Subscription.API.Models.Entities;

namespace Subscription.API.Matching.Interfaces
{
	public interface IAdvertSubscriptionMatcher
	{
		bool IsMatch(AdvertSubscriptionEntity subscription, AdvertCreatedEvent @event);
	}
}
