using BuildingBlocks.Infrastructure;
using DocumentMetadata.API.Models.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DocumentMetadata.API.Models
{
	public class DocumentMetadata : AuditableEntity, IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public Guid Id { get; set; }

		public string Url { get; set; } = string.Empty;

		public string? ThumbnailUrl { get; set; }

		public string FileName { get; set; } = string.Empty;

		public string MimeType { get; set; } = string.Empty;

		[BsonRepresentation(BsonType.String)]
		public Guid? OwnerUserId { get; set; }

		public DocumentType DocumentType { get; set; }

		[BsonRepresentation(BsonType.String)]
		public Guid OwnerObjectId { get; set; }

		public OwnerObjectType OwnerObjectType { get; set; }
	}
}
