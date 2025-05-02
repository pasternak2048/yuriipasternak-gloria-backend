using Advert.API.Models.Enums;
using BuildingBlocks.Filtering;

namespace Advert.API.Models.Filters
{
	public class AdvertFilters : BaseFilters
	{
		public Guid? RealtyId { get; set; }
		public AdvertType? AdvertType { get; set; }
		public AdvertStatus? Status { get; set; }

		public override string CacheKey() =>
			$"realty={RealtyId?.ToString() ?? "any"}:type={AdvertType?.ToString() ?? "any"}:status={Status?.ToString() ?? "any"}";
	}
}
