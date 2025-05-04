using BuildingBlocks.Common.Enums;

namespace Advert.API.Models.DTOs.Requests
{
	public class AdvertCreateRequest
	{
		public Guid RealtyId { get; set; }

		public AdvertType AdvertType { get; set; }

		public decimal Price { get; set; }

		public CurrencyCode Currency { get; set; } = CurrencyCode.UAH;

		public string Title { get; set; } = string.Empty;

		public string? Description { get; set; }
	}
}
