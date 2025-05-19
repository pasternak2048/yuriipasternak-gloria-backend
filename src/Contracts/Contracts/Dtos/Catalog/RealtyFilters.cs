using Contracts.Dtos.Common;
using Contracts.Enums;

namespace Contracts.Dtos.Catalog
{
	public class RealtyFilters : BaseFilters
	{
		public RealtyType? Type { get; set; }

		public RealtyStatus? Status { get; set; }

		public override string CacheKey() =>
		$"type={Type?.ToString() ?? "any"}:status={Status?.ToString() ?? "any"}";
	}
}
