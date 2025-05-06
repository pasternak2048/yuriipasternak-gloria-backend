using Notification.API.Models.Entities;

namespace Notification.API.Repositories
{
	public interface INotificationRepository
	{
		Task CreateAsync(NotificationEntity notification, CancellationToken cancellationToken);
	}
}
