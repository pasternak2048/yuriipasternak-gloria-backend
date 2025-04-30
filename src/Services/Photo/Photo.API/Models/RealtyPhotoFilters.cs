using BuildingBlocks.Filtering;

namespace Photo.API.Models
{
	public class RealtyPhotoFilters : Filters
	{
		public Guid? RealtyId { get; set; }

		public override string CacheKey()
		{
			return $"realtyphoto:{RealtyId}";
		}
	}
}
