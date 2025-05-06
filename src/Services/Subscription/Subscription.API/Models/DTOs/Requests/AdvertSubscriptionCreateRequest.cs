using BuildingBlocks.Common.Enums;

namespace Subscription.API.Models.DTOs.Requests
{
	public class AdvertSubscriptionCreateRequest
	{
		public NotificationEventType EventType { get; set; } = NotificationEventType.AdvertCreated;

		public AdvertType AdvertType { get; set; } = AdvertType.Rent;

		public string Title { get; set; } = string.Empty;

		public string Street { get; set; } = string.Empty;

		public string City { get; set; } = string.Empty;

		public string Region { get; set; } = string.Empty;

		public decimal MinPrice { get; set; }

		public decimal MaxPrice { get; set; }

		public CurrencyCode Currency { get; set; } = CurrencyCode.UAH;
	}
}
