using Contracts.Dtos.Common;
using Contracts.Dtos.Notification;
using Notification.API.Models.Entities;

namespace Notification.API.Repositories
{
	public interface INotificationRepository
	{
		Task CreateAsync(NotificationEntity notification, CancellationToken cancellationToken);

		Task<PaginatedResult<NotificationEntity>> GetPaginatedAsync(NotificationFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken);

		Task<NotificationEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

		Task<NotificationEntity?> MarkAsReadAsync(Guid id, CancellationToken cancellationToken);

		Task<int> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken);
	}
}
