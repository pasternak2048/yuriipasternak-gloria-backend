using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using View.Domain.Entities.Base;

namespace View.Domain.Entities
{
	public class View : AuditableEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public Guid Id { get; set; }

		[BsonRepresentation(BsonType.String)]
		public Guid RealtyId { get; set; }

		[BsonRepresentation(BsonType.String)]
		public Guid ClientId { get; set; }

		[BsonRepresentation(BsonType.String)]
		public Guid AgentId { get; set; }

		public DateTime ScheduledAt { get; set; }

		public string Location { get; set; } = string.Empty;

		public string Notes { get; set; } = string.Empty;
	}
}
