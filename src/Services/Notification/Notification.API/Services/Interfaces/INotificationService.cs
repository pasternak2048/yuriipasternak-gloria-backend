using Notification.API.Models.Entities;

namespace Notification.API.Services.Interfaces
{
	public interface INotificationService
	{
		Task CreateAsync(NotificationEntity notification, CancellationToken cancellationToken);
	}
}
