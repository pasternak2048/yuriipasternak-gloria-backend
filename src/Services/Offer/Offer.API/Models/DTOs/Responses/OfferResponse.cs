using Offer.API.Models.Enums;

namespace Offer.API.Models.DTOs.Responses
{
	public class OfferResponse
	{
		public Guid Id { get; set; }

		public Guid RealtyId { get; set; }

		public OfferType OfferType { get; set; }

		public decimal Price { get; set; }

		public string Currency { get; set; } = "USD";

		public string Title { get; set; } = string.Empty;

		public string? Description { get; set; }

		public OfferStatus Status { get; set; }

		public Address Address { get; set; } = new();

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public string? CreatedBy { get; set; }

		public string? UpdatedBy { get; set; }
	}
}
