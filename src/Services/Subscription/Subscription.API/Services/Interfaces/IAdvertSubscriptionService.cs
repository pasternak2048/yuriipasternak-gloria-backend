using Contracts.Dtos.Subscription;
using Contracts.Enums;
using Subscription.API.Models.Entities;

namespace Subscription.API.Services.Interfaces
{
	public interface IAdvertSubscriptionService
	{
		Task CreateAsync(AdvertSubscriptionCreateRequest request, CancellationToken cancellationToken);

		Task<List<AdvertSubscriptionEntity>> GetByEventTypeAsync(NotificationEventType eventType, CancellationToken cancellationToken);
	}
}
