using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Enums;
using MongoDB.Driver;

namespace GLORIA.Contracts.Dtos.Catalog
{
	public class RealtyFilters : BaseFilters
	{
		public RealtyType? Type { get; set; }

		public RealtyStatus? Status { get; set; }

		public override string CacheKey() =>
		$"type={Type?.ToString() ?? "any"}:status={Status?.ToString() ?? "any"}";

		public override FilterDefinition<RealtyEntity> ToFilter<RealtyEntity>()
		{
			var builder = Builders<RealtyEntity>.Filter;
			var filter = builder.Empty;

			if (Type.HasValue)
				filter &= builder.Eq("Type", Type.Value);

			if (Status.HasValue)
				filter &= builder.Eq("Status", Status.Value);

			return filter;
		}
	}
}
