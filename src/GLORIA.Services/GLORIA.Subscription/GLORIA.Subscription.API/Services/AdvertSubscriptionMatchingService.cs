using GLORIA.Contracts.Events;
using GLORIA.Subscription.API.Matching.Interfaces;
using GLORIA.Subscription.API.Models.Entities;
using GLORIA.Subscription.API.Repositories.Interfaces;

namespace GLORIA.Subscription.API.Services
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
