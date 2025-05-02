using BuildingBlocks.Infrastructure;
using Offer.API.Models.Enums;

namespace Offer.API.Models
{
	public class Offer : AuditableEntity, IEntity
	{
		public Guid Id { get; set; }

		public Guid RealtyId { get; set; }

		public OfferType OfferType { get; set; }

		public decimal Price { get; set; }

		public string Currency { get; set; } = "USD";

		public string Title { get; set; } = string.Empty;

		public string? Description { get; set; }

		public OfferStatus Status { get; set; } = OfferStatus.Active;

		public Address Address { get; set; } = new();
	}
}
