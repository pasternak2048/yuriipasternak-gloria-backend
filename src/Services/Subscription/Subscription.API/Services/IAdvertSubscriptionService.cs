using BuildingBlocks.Common.Enums;
using Subscription.API.Models.DTOs.Requests;
using Subscription.API.Models.Entities;

namespace Subscription.API.Services
{
	public interface IAdvertSubscriptionService
	{
		Task CreateAsync(AdvertSubscriptionCreateRequest request, CancellationToken cancellationToken);

		Task<List<AdvertSubscriptionEntity>> GetByEventTypeAsync(NotificationEventType eventType, CancellationToken cancellationToken);
	}
}
