using Contracts.Events;
using Subscription.API.Matching.Interfaces;
using Subscription.API.Models.Entities;

namespace Subscription.API.Matching
{
	public class AdvertSubscriptionMatcher : IAdvertSubscriptionMatcher
	{
		public bool IsMatch(AdvertSubscriptionEntity s, AdvertCreatedEvent e)
		{
			return
				s.AdvertType == e.AdvertType &&
				(s.Street == e.Street || string.IsNullOrWhiteSpace(s.Street)) &&
				(s.City == e.City || string.IsNullOrWhiteSpace(s.City)) &&
				(s.Region == e.Region || string.IsNullOrWhiteSpace(s.Region)) &&
				(e.Price >= s.MinPrice && e.Price <= s.MaxPrice) &&
				s.Currency == e.Currency;
		}
	}
}
