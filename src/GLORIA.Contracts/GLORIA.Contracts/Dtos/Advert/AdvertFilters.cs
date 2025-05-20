using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Enums;
using MongoDB.Driver;

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

		public bool OnlyActiveOrInactive { get; set; }

		public override string CacheKey() =>
			$"realty={RealtyId?.ToString() ?? "any"}:" +
			$"type={AdvertType?.ToString() ?? "any"}:" +
			$"minPrice={MinPrice?.ToString() ?? "any"}:" +
			$"maxPrice={MaxPrice?.ToString() ?? "any"}:" +
			$"status={Status?.ToString() ?? "any"}:" +
			$"city={City ?? "any"}:" +
			$"region={Region ?? "any"}:" +
			$"street={Street ?? "any"}:" +
			$"zipCode={ZipCode ?? "any"}:" +
			$"activeOrInactive={OnlyActiveOrInactive}";

		public override FilterDefinition<AdvertEntity> ToFilter<AdvertEntity>()
		{
			var builder = Builders<AdvertEntity>.Filter;
			var filter = builder.Empty;

			if (RealtyId.HasValue)
				filter &= builder.Eq("RealtyId", RealtyId.Value);

			if (AdvertType.HasValue)
				filter &= builder.Eq("AdvertType", AdvertType.Value);

			if (MinPrice.HasValue)
				filter &= builder.Gte("Price", MinPrice.Value);

			if (MaxPrice.HasValue)
				filter &= builder.Lte("Price", MaxPrice.Value);

			if (Status.HasValue)
				filter &= builder.Eq("Status", Status.Value);

			if (!string.IsNullOrWhiteSpace(City))
				filter &= builder.Eq("City", City);

			if (!string.IsNullOrWhiteSpace(Region))
				filter &= builder.Eq("Region", Region);

			if (!string.IsNullOrWhiteSpace(Street))
				filter &= builder.Eq("Street", Street);

			if (!string.IsNullOrWhiteSpace(ZipCode))
				filter &= builder.Eq("ZipCode", ZipCode);

			if (OnlyActiveOrInactive)
				filter &= builder.In("Status", new[] { AdvertStatus.Active, AdvertStatus.Inactive });

			return filter;
		}
	}
}
