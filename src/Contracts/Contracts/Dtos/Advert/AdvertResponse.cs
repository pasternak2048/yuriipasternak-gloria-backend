using Contracts.Enums;

namespace Contracts.Dtos.Advert
{
	public class AdvertResponse
	{
		public Guid Id { get; set; }

		public Guid RealtyId { get; set; }

		public AdvertType AdvertType { get; set; }

		public decimal Price { get; set; }

		public CurrencyCode Currency { get; set; } = CurrencyCode.UAH;

		public string Title { get; set; } = string.Empty;

		public string? Description { get; set; }

		public AdvertStatus Status { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? ModifiedAt { get; set; }

		public Guid CreatedBy { get; set; }

		public string FullAddress { get; set; } = string.Empty;
	}
}
