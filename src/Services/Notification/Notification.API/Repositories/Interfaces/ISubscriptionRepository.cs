using Notification.API.Models.Entities;
using Notification.API.Models.Enums;

namespace Notification.API.Repositories.Interfaces
{
	public interface ISubscriptionRepository
	{
		Task CreateAsync(NotificationSubscription subscription, CancellationToken cancellationToken);

		Task<List<NotificationSubscription>> GetByEventTypeAsync(NotificationEventType eventType, CancellationToken cancellationToken);
	}
}
