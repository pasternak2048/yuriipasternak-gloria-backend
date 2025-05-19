using GLORIA.Subscription.API.Models.Entities;

namespace GLORIA.Subscription.API.Repositories.Interfaces
{
	public interface IAdvertSubscriptionLookupRepository
	{
		Task<IReadOnlyCollection<AdvertSubscriptionEntity>> GetAllAsync(CancellationToken cancellationToken);
	}
}
