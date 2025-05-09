using BuildingBlocks.Pagination;
using Notification.API.Models.DTOs.Requests;
using Notification.API.Models.Entities;
using Notification.API.Models.Filters;

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
