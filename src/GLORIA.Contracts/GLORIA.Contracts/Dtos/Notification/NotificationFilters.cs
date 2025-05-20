using GLORIA.Contracts.Dtos.Common;
using MongoDB.Driver;

namespace GLORIA.Contracts.Dtos.Notification
{
	public class NotificationFilters : BaseFilters
	{
		public Guid? UserId { get; set; }

		public bool? IsRead { get; set; }

		public override string CacheKey() =>
			$"user:{UserId?.ToString() ?? "any"}:" +
			$"user:{IsRead?.ToString() ?? "any"}";

		public override FilterDefinition<NotificationEntity> ToFilter<NotificationEntity>()
		{
			var builder = Builders<NotificationEntity>.Filter;
			var filter = builder.Empty;

			if (UserId.HasValue)
				filter &= builder.Eq("UserId", UserId.Value);

			if (IsRead.HasValue)
				filter &= builder.Eq("IsRead", IsRead.Value);

			return filter;
		}
	}
}
