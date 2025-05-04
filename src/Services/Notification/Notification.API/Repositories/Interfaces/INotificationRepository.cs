using NotificationEntity = Notification.API.Models.Entities.Notification;

namespace Notification.API.Repositories.Interfaces
{
	public interface INotificationRepository
	{
		Task CreateAsync(NotificationEntity notification, CancellationToken cancellationToken);
	}
}
