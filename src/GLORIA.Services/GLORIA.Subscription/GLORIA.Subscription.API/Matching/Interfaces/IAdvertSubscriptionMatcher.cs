using GLORIA.Contracts.Events;
using GLORIA.Subscription.API.Models.Entities;

namespace GLORIA.Subscription.API.Matching.Interfaces
{
	public interface IAdvertSubscriptionMatcher
	{
		bool IsMatch(AdvertSubscriptionEntity subscription, AdvertCreatedEvent @event);
	}
}
