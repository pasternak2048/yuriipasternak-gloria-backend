using BuildingBlocks.Common.Enums;
using BuildingBlocks.Infrastructure.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Subscription.API.Models.Entities
{
	public class SubscriptionEntity : AuditableEntity, IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public Guid Id { get; set; }

		[BsonRepresentation(BsonType.String)]
		public Guid UserId { get; set; }

		public NotificationEventType EventType { get; set; }

		public string FilterJson { get; set; } = string.Empty;
	}
}
