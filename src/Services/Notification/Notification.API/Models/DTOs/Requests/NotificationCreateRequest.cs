using BuildingBlocks.Common.Enums;

namespace Notification.API.Models.DTOs.Requests
{
	public class NotificationCreateRequest
	{
		public Guid UserId { get; set; }

		public NotificationEventType EventType { get; set; }

		public string Title { get; set; } = string.Empty;

		public string Message { get; set; } = string.Empty;
	}
}
