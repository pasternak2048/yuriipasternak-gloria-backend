using GLORIA.Contracts.Enums;

namespace GLORIA.Contracts.Dtos.Notification
{
	public class NotificationCreateRequest
	{
		public Guid UserId { get; set; }

		public NotificationEventType EventType { get; set; }

		public string Title { get; set; } = string.Empty;

		public string Message { get; set; } = string.Empty;
	}
}
