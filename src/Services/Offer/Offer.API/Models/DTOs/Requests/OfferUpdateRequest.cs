using Offer.API.Models.Enums;

namespace Offer.API.Models.DTOs.Requests
{
	public class OfferUpdateRequest
	{
		public OfferType OfferType { get; set; }

		public decimal Price { get; set; }

		public string Currency { get; set; } = "USD";

		public string Title { get; set; } = string.Empty;

		public string? Description { get; set; }

		public OfferStatus Status { get; set; } = OfferStatus.Active;

		public Address Address { get; set; } = new();
	}
}
