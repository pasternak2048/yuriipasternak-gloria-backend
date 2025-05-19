using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Dtos.Notification;
using GLORIA.Notification.API.Models.Entities;

namespace GLORIA.Notification.API.Services.Interfaces
{
	public interface INotificationService
	{
		Task CreateAsync(NotificationCreateRequest request, CancellationToken cancellationToken);

		Task<PaginatedResult<NotificationEntity>> GetPaginatedAsync(NotificationFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken);

		Task<NotificationEntity?> MarkAsReadAsync(Guid id, CancellationToken cancellationToken);

		Task<int> MarkAllAsReadAsync(CancellationToken cancellationToken);
	}
}
