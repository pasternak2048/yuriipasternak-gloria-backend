using GLORIA.Contracts.Dtos.Subscription;
using GLORIA.Contracts.Enums;
using GLORIA.Subscription.API.Models.Entities;

namespace GLORIA.Subscription.API.Services.Interfaces
{
	public interface IAdvertSubscriptionService
	{
		Task CreateAsync(AdvertSubscriptionCreateRequest request, CancellationToken cancellationToken);

		Task<List<AdvertSubscriptionEntity>> GetByEventTypeAsync(NotificationEventType eventType, CancellationToken cancellationToken);
	}
}
