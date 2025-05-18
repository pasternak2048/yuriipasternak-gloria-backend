using BuildingBlocks.Common.Enums;
using BuildingBlocks.Infrastructure.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Subscription.API.Models.Entities
{
	public class AdvertSubscriptionEntity : AuditableEntity, IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public Guid Id { get; set; }

		[BsonRepresentation(BsonType.String)]
		public Guid UserId { get; set; }

		public NotificationEventType EventType { get; set; } = NotificationEventType.AdvertCreated;

		public AdvertType AdvertType { get; set; } = AdvertType.Rent;

		public string Title { get; set; } = string.Empty;

		public string Street { get; set; } = string.Empty;

		public string City { get; set; } = string.Empty;

		public string Region { get; set; } = string.Empty;

		public decimal MinPrice { get; set; }

		public decimal MaxPrice { get; set; }

		public CurrencyCode Currency { get; set; } = CurrencyCode.UAH;
	}
}
