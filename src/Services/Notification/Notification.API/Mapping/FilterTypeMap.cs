using BuildingBlocks.Common.Enums;
using Notification.API.Models.Filters.Advert;

namespace Notification.API.Mapping
{
	public static class FilterTypeMap
	{
		public static Type GetFilterType(NotificationEventType eventType) =>
			eventType switch
			{
				NotificationEventType.AdvertCreated => typeof(AdvertCreatedFilter),
				_ => throw new NotSupportedException()
			};
	}
}
