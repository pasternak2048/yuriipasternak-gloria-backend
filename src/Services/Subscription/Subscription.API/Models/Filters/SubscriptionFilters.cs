using BuildingBlocks.Common.Enums;
using BuildingBlocks.Filtering;

namespace Subscription.API.Models.Filters
{
	public class SubscriptionFilters : BaseFilters
	{
		public Guid? UserId { get; set; }

		public NotificationEventType? EventType { get; set; }

		public string? FilterJson { get; set; }

		public override string CacheKey()
		{
			return $"user:{UserId?.ToString() ?? "any"};" +
				   $"event:{EventType?.ToString() ?? "any"};" +
				   $"filter:{FilterJson ?? "none"}";
		}
	}
}
