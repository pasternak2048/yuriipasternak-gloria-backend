using Contracts.Dtos.Common;
using Contracts.Dtos.Notification;
using Notification.API.Models.Entities;

namespace Notification.API.Services.Interfaces
{
	public interface INotificationService
	{
		Task CreateAsync(NotificationCreateRequest request, CancellationToken cancellationToken);

		Task<PaginatedResult<NotificationEntity>> GetPaginatedAsync(NotificationFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken);

		Task<NotificationEntity?> MarkAsReadAsync(Guid id, CancellationToken cancellationToken);

		Task<int> MarkAllAsReadAsync(CancellationToken cancellationToken);
	}
}
