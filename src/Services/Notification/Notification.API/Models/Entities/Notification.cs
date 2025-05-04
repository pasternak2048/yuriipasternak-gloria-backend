using BuildingBlocks.Infrastructure.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Notification.API.Models.Enums;

namespace Notification.API.Models.Entities
{
	public class Notification : IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public NotificationEventType EventType { get; set; }

		public string Title { get; set; } = string.Empty;

		public string Message { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public bool IsRead { get; set; } = false;
	}
}
