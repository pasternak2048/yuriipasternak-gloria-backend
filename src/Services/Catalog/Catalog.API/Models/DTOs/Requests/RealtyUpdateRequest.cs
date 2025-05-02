using BuildingBlocks.Common.DTOs;
using Catalog.API.Models.Entities;
using Catalog.API.Models.Enums;

namespace Catalog.API.Models.DTOs.Requests
{
	public class RealtyUpdateRequest
	{
		public RealtyType Type { get; set; }

		public WallType WallType { get; set; }

		public HeatingType HeatingType { get; set; }

		public RealtyStatus RealtyStatus { get; set; } = RealtyStatus.Draft;

		public double Area { get; set; }

		public int Floor { get; set; }

		public int Rooms { get; set; }

		public int Baths { get; set; }

		public DateTimeOffset BuildDate { get; set; }

		public Address Address { get; set; } = new();
	}
}
