using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Enums;
using MongoDB.Driver;

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

		public override FilterDefinition<AdvertSubscriptionEntity> ToFilter<AdvertSubscriptionEntity>()
		{
			var builder = Builders<AdvertSubscriptionEntity>.Filter;
			var filter = builder.Empty;

			if (UserId.HasValue)
				filter &= builder.Eq("UserId", UserId.Value);

			if (EventType.HasValue)
				filter &= builder.Eq("EventType", EventType.Value);

			if (AdvertType.HasValue)
				filter &= builder.Eq("AdvertType", AdvertType.Value);

			if (!string.IsNullOrWhiteSpace(Title))
				filter &= builder.Eq("Title", Title);

			if (!string.IsNullOrWhiteSpace(Street))
				filter &= builder.Eq("Street", Street);

			if (!string.IsNullOrWhiteSpace(City))
				filter &= builder.Eq("City", City);

			if (!string.IsNullOrWhiteSpace(Region))
				filter &= builder.Eq("Region", Region);

			if (MinPrice.HasValue)
				filter &= builder.Gte("MinPrice", MinPrice.Value);

			if (MaxPrice.HasValue)
				filter &= builder.Lte("MaxPrice", MaxPrice.Value);

			if (Currency.HasValue)
				filter &= builder.Eq("Currency", Currency.Value);

			return filter;
		}
	}
}
