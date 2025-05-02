using BuildingBlocks.Filtering;
using Offer.API.Models.Enums;

namespace Offer.API.Models
{
	public class OfferFilters : BaseFilters
	{
		public Guid? RealtyId { get; set; }

		public OfferType? OfferType { get; set; }

		public OfferStatus? Status { get; set; }

		public decimal? MinPrice { get; set; }

		public decimal? MaxPrice { get; set; }

		public override string CacheKey() =>
			$"realtyId={RealtyId?.ToString() ?? "any"}:" +
			$"offerType={OfferType?.ToString() ?? "any"}:" +
			$"status={Status?.ToString() ?? "any"}:" +
			$"minPrice={(MinPrice.HasValue ? MinPrice.Value.ToString("F2") : "any")}:" +
			$"maxPrice={(MaxPrice.HasValue ? MaxPrice.Value.ToString("F2") : "any")}";
	}
}
