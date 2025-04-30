using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DocumentMetadata.API.Models
{
	public abstract class AuditableEntity
	{
		[BsonRepresentation(BsonType.String)]
		public Guid? CreatedBy { get; set; }

		public DateTime CreatedAt { get; set; }

		[BsonRepresentation(BsonType.String)]
		public Guid? ModifiedBy { get; set; }

		public DateTime? ModifiedAt { get; set; }
	}
}
