using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Enums;

namespace GLORIA.Contracts.Dtos.Advert
{
	public class AdvertUpdateRequest
	{
		public AdvertType AdvertType { get; set; }

		public decimal Price { get; set; }

		public CurrencyCode Currency { get; set; } = CurrencyCode.UAH;

		public string Title { get; set; } = string.Empty;

		public string? Description { get; set; }

		public AdvertStatus Status { get; set; }

		public Address Address { get; set; } = new();
	}
}
