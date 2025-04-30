using BuildingBlocks.Filtering;
using Catalog.API.Models.Enums;

namespace Catalog.API.Models
{
	public class RealtyFilters : Filters
	{
		public RealtyType? Type { get; set; }

		public RealtyStatus? Status { get; set; }

		public override string CacheKey() =>
		$"type={Type?.ToString() ?? "any"}:status={Status?.ToString() ?? "any"}";
	}
}
