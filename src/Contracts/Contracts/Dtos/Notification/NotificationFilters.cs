using Contracts.Dtos.Common;

namespace Contracts.Dtos.Notification
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
