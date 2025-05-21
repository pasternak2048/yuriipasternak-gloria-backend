using GLORIA.Contracts.Enums;

namespace GLORIA.Contracts.Dtos.Notification
{
    public class NotificationResponse
    {
        public Guid Id { get; set; }

        public NotificationEventType EventType { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }

        public NotificationObject? Object { get; set; }
    }
}
