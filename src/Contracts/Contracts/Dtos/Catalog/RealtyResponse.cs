using Contracts.Enums;

namespace Contracts.Dtos.Catalog
{
	public class RealtyResponse
	{
		public Guid Id { get; set; }

		public RealtyType Type { get; set; }

		public WallType WallType { get; set; }

		public HeatingType HeatingType { get; set; }

		public RealtyStatus Status { get; set; }

		public double Area { get; set; }

		public int Floor { get; set; }

		public int Rooms { get; set; }

		public int Baths { get; set; }

		public DateTime BuildDate { get; set; }

		public string FullAddress { get; set; } = string.Empty;
	}
}
