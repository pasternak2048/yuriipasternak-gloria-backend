using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Abstractions.Entities;
using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GLORIA.Catalog.API.Models.Entities
{
	public class RealtyEntity : AuditableEntity, IEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.String)]
		public Guid Id { get; set; }

		public RealtyType Type { get; set; }

		public WallType WallType { get; set; }

		public HeatingType HeatingType { get; set; }
		 
		public RealtyStatus Status { get; set; } = RealtyStatus.Draft;

		public double Area { get; set; }

		public int Floor { get; set; }

		public int Rooms { get; set; }

		public int Baths { get; set; }

		[BsonRepresentation(BsonType.DateTime)]
		public DateTime BuildDate { get; set; }

		public Address Address { get; set; } = new();
	}
}
