using Subscription.API.Models.Entities;

namespace Subscription.API.Repositories.Interfaces
{
	public interface IAdvertSubscriptionLookupRepository
	{
		Task<IReadOnlyCollection<AdvertSubscriptionEntity>> GetAllAsync(CancellationToken cancellationToken);
	}
}
