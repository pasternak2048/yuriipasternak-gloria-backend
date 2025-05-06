using BuildingBlocks.Common.Enums;

namespace Notification.API.Models.Filters.Advert
{
	public class AdvertCreatedFilter
	{
		public string? Street { get; set; }

		public string? City { get; set; }

		public string? Region { get; set; }

		public AdvertType? AdvertType { get; set; }

		public decimal? Price { get; set; }

		public CurrencyCode? Currency { get; set; }
	}
}
