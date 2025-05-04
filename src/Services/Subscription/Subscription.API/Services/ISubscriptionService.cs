using BuildingBlocks.Common.Enums;
using Subscription.API.Models.DTOs.Requests;
using Subscription.API.Models.Entities;

namespace Subscription.API.Services
{
	public interface ISubscriptionService
	{
		Task CreateAsync(SubscriptionCreateRequest request, CancellationToken cancellationToken);

		Task<List<SubscriptionEntity>> GetByEventTypeAsync(NotificationEventType eventType, CancellationToken cancellationToken);
	}
}
