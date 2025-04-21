using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Photo.API.Models.Base
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