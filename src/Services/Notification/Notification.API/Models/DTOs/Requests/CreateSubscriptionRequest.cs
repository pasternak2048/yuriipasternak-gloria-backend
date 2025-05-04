using Notification.API.Models.Enums;
using System.Text.Json;

namespace Notification.API.Models.DTOs.Requests
{
	public class CreateSubscriptionRequest
	{
		public NotificationEventType EventType { get; set; }

		public JsonElement Filter { get; set; }
	}
}
