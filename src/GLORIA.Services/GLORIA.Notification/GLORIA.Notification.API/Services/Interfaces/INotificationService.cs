using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Dtos.Notification;

namespace GLORIA.Notification.API.Services.Interfaces
{
	public interface INotificationService
	{
		Task CreateAsync(NotificationCreateRequest request, CancellationToken cancellationToken);

		Task<PaginatedResult<NotificationResponse>> GetPaginatedAsync(NotificationFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken);

		Task<NotificationResponse?> MarkAsReadAsync(Guid id, CancellationToken cancellationToken);

		Task<int> MarkAllAsReadAsync(CancellationToken cancellationToken);
	}
}
