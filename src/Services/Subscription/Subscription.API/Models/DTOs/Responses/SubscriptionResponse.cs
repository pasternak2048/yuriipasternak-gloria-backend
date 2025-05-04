using BuildingBlocks.Common.Enums;

namespace Subscription.API.Models.DTOs.Responses
{
	public class SubscriptionResponse
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public NotificationEventType EventType { get; set; }

		public string FilterJson { get; set; } = string.Empty;
	}
}
