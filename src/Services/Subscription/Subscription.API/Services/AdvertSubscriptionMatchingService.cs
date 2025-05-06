using Contracts.Events;
using Subscription.API.Matching.Interfaces;
using Subscription.API.Models.Entities;
using Subscription.API.Repositories.Interfaces;

namespace Subscription.API.Services
{
	public class AdvertSubscriptionMatchingService
	{
		private readonly IAdvertSubscriptionLookupRepository _repository;
		private readonly IAdvertSubscriptionMatcher _matcher;

		public AdvertSubscriptionMatchingService(
			IAdvertSubscriptionLookupRepository repository,
			IAdvertSubscriptionMatcher matcher)
		{
			_repository = repository;
			_matcher = matcher;
		}

		public async Task<IReadOnlyCollection<AdvertSubscriptionEntity>> GetMatchingSubscriptionsAsync(
			AdvertCreatedEvent @event,
			CancellationToken cancellationToken)
		{
			var all = await _repository.GetAllAsync(cancellationToken);
			return all.Where(s => _matcher.IsMatch(s, @event)).ToArray();
		}
	}
}
