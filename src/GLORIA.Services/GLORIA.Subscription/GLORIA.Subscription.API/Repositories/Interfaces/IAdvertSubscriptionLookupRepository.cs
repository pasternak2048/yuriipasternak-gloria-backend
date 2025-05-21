using GLORIA.Contracts.Events;
using GLORIA.Subscription.API.Models.Entities;

namespace GLORIA.Subscription.API.Repositories.Interfaces
{
	public interface IAdvertSubscriptionLookupRepository
	{
        Task<IReadOnlyCollection<AdvertSubscriptionEntity>> GetMatchingAsync(AdvertCreatedEvent @event, CancellationToken cancellationToken);
    }
}
