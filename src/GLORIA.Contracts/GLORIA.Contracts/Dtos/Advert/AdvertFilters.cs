using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Enums;

namespace GLORIA.Contracts.Dtos.Advert
{
	public class AdvertFilters : BaseFilters
	{
		public Guid? RealtyId { get; set; }

		public AdvertType? AdvertType { get; set; }

		public decimal? MinPrice { get; set; }

		public decimal? MaxPrice { get; set; }

		public AdvertStatus? Status { get; set; }

		public string? City { get; set; }

		public string? Region { get; set; }

		public string? Street { get; set; }

		public string? ZipCode { get; set; }

		public override string CacheKey() =>
			$"realty={RealtyId?.ToString() ?? "any"}:" +
			$"type={AdvertType?.ToString() ?? "any"}:" +
			$"minPrice={MinPrice?.ToString() ?? "any"}:" +
			$"maxPrice={MaxPrice?.ToString() ?? "any"}:" +
			$"status={Status?.ToString() ?? "any"}:" +
			$"city={City ?? "any"}:" +
			$"region={Region ?? "any"}:" +
			$"street={Street ?? "any"}:" +
			$"zipCode={ZipCode ?? "any"}";

	}
}
