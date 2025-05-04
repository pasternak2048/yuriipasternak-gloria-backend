using NotificationEntity = Notification.API.Models.Entities.Notification;

namespace Notification.API.Services.Interfaces
{
	public interface INotificationService
	{
		Task CreateAsync(NotificationEntity notification, CancellationToken cancellationToken);
	}
}
