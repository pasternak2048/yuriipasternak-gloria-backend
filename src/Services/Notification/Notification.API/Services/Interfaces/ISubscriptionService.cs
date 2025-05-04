using Notification.API.Models.DTOs.Requests;
using Notification.API.Models.Entities;
using Notification.API.Models.Enums;

namespace Notification.API.Services.Interfaces
{
	public interface ISubscriptionService
	{
		Task CreateAsync(CreateSubscriptionRequest request, CancellationToken cancellationToken);

		Task<List<NotificationSubscription>> GetByEventTypeAsync(NotificationEventType eventType, CancellationToken cancellationToken);
	}
}
