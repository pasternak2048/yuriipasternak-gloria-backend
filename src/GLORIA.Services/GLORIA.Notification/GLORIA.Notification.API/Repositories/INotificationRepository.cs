using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Dtos.Notification;
using GLORIA.Notification.API.Models.Entities;

namespace GLORIA.Notification.API.Repositories
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
