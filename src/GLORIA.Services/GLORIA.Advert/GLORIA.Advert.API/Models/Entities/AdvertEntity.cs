using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Abstractions.Entities;
using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GLORIA.Advert.API.Models.Entities
{
	public class AdvertEntity : AuditableEntity, IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public Guid Id { get; set; }

		[BsonRepresentation(BsonType.String)]
		public Guid RealtyId { get; set; }

		public AdvertType AdvertType { get; set; }

		public decimal Price { get; set; }

		public CurrencyCode Currency { get; set; } = CurrencyCode.UAH;

		public string Title { get; set; } = string.Empty;

		public string? Description { get; set; }

		public AdvertStatus Status { get; set; } = AdvertStatus.Active;

		public Address Address { get; set; } = new();
	}
}
