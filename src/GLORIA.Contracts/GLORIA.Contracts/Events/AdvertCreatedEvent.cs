using GLORIA.Contracts.Enums;

namespace GLORIA.Contracts.Events
{
	public class AdvertCreatedEvent
	{
		public Guid AdvertId { get; set; }

		public Guid RealtyId { get; set; }

		public AdvertType AdvertType { get; set; } = AdvertType.Rent;

		public string Title { get; set; } = string.Empty;

		public string Street { get; set; } = string.Empty;

		public string City { get; set; } = string.Empty;

		public string Region { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public CurrencyCode Currency { get; set; } = CurrencyCode.UAH;

		public DateTime CreatedAt { get; set; }
	}
}
