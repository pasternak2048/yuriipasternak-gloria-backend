using BuildingBlocks.Common.Enums;
using System.Text.Json;

namespace Subscription.API.Models.DTOs.Requests
{
	public class SubscriptionCreateRequest
	{
		public NotificationEventType EventType { get; set; }

		public JsonElement Filter { get; set; }
	}
}
