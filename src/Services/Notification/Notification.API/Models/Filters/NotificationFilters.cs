using BuildingBlocks.Filtering;

namespace Notification.API.Models.Filters
{
	public class NotificationFilters : BaseFilters
	{
		public Guid? UserId { get; set; }

		public bool? IsRead { get; set; }

		public override string CacheKey() =>
			$"user:{UserId?.ToString() ?? "any"}:" +
			$"user:{IsRead?.ToString() ?? "any"}";
	}
}
