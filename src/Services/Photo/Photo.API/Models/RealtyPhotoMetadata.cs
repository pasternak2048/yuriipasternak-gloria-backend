using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Photo.API.Models.Base;
using BuildingBlocks.Infrastructure;

namespace Photo.API.Models
{
	public class RealtyPhotoMetadata : AuditableEntity, IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public Guid Id { get; set; }

		[BsonRepresentation(BsonType.String)]
		public Guid RealtyId { get; set; }

		public string FileName { get; set; } = string.Empty;

		public string ContentType { get; set; } = string.Empty;

		public string Url { get; set; } = string.Empty;

		public string ThumbnailUrl { get; set; } = string.Empty;
	}
}
