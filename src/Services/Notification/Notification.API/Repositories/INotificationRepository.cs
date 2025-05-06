using NotificationEntity = Notification.API.Models.Entities.Notification;

namespace Notification.API.Repositories
{
	public interface INotificationRepository
	{
		Task CreateAsync(NotificationEntity notification, CancellationToken cancellationToken);
	}
}
