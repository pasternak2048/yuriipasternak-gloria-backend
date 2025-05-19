using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Enums;

namespace GLORIA.Contracts.Dtos.Subscription
{
	public class AdvertSubscriptionFilters : BaseFilters
	{
		public Guid? UserId { get; set; }

		public NotificationEventType? EventType { get; set; }

		public AdvertType? AdvertType { get; set; }

		public string? Title { get; set; }

		public string? Street { get; set; }

		public string? City { get; set; }

		public string? Region { get; set; }

		public decimal? MinPrice { get; set; }

		public decimal? MaxPrice { get; set; }

		public CurrencyCode? Currency { get; set; }


		public override string CacheKey()
		{
			return $"user:{UserId?.ToString() ?? "any"}:" +
				   $"event:{EventType?.ToString() ?? "any"}:" +
				   $"type:{AdvertType?.ToString() ?? "any"}:" +
				   $"title:{Title ?? "any"}:" +
				   $"street:{Street ?? "any"}:" +
				   $"city:{City ?? "any"}:" +
				   $"region:{Region ?? "any"}:" +
				   $"minPrice:{MinPrice?.ToString() ?? "any"}:" +
				   $"maxPrice:{MaxPrice?.ToString() ?? "any"}:" +
				   $"currency:{Currency?.ToString() ?? "any"}";
		}
	}
}
