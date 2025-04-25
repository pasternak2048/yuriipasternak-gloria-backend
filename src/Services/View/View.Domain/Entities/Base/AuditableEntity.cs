using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace View.Domain.Entities.Base
{
	public abstract class AuditableEntity
	{
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		[BsonRepresentation(BsonType.String)]
		public Guid? CreatedBy { get; set; }

		public DateTime? ModifiedAt { get; set; }

		[BsonRepresentation(BsonType.String)]
		public Guid? ModifiedBy { get; set; }
	}
}
