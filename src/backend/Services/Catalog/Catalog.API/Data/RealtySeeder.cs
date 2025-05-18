using BuildingBlocks.Common.DTOs;
using BuildingBlocks.Configuration;
using BuildingBlocks.Persistence.Mongo;
using Catalog.API.Models.Entities;
using Catalog.API.Models.Enums;
using MongoDB.Driver;

namespace Catalog.API.Data
{
	public class RealtySeeder : ICollectionSeeder<RealtyEntity>
	{
		private readonly List<Guid> RealtyIds = new()
		{
			Guid.Parse("00000000-0000-0000-0000-000000000001"),
			Guid.Parse("00000000-0000-0000-0000-000000000002"),
			Guid.Parse("00000000-0000-0000-0000-000000000003"),
			Guid.Parse("00000000-0000-0000-0000-000000000004"),
			Guid.Parse("00000000-0000-0000-0000-000000000005"),
			Guid.Parse("00000000-0000-0000-0000-000000000006"),
			Guid.Parse("00000000-0000-0000-0000-000000000007"),
			Guid.Parse("00000000-0000-0000-0000-000000000008"),
			Guid.Parse("00000000-0000-0000-0000-000000000009"),
			Guid.Parse("00000000-0000-0000-0000-000000000010"),
			Guid.Parse("00000000-0000-0000-0000-000000000011"),
			Guid.Parse("00000000-0000-0000-0000-000000000012"),
			Guid.Parse("00000000-0000-0000-0000-000000000013"),
			Guid.Parse("00000000-0000-0000-0000-000000000014"),
			Guid.Parse("00000000-0000-0000-0000-000000000015"),
			Guid.Parse("00000000-0000-0000-0000-000000000016"),
			Guid.Parse("00000000-0000-0000-0000-000000000017"),
			Guid.Parse("00000000-0000-0000-0000-000000000018"),
			Guid.Parse("00000000-0000-0000-0000-000000000019"),
			Guid.Parse("00000000-0000-0000-0000-000000000020"),
			Guid.Parse("00000000-0000-0000-0000-000000000021"),
			Guid.Parse("00000000-0000-0000-0000-000000000022"),
			Guid.Parse("00000000-0000-0000-0000-000000000023"),
			Guid.Parse("00000000-0000-0000-0000-000000000024"),
			Guid.Parse("00000000-0000-0000-0000-000000000025")
		};

		public async Task SeedAsync(IMongoCollection<RealtyEntity> collection)
		{
			var items = new List<RealtyEntity>();
			for (int i = 0; i < RealtyIds.Count; i++)
			{
				items.Add(new RealtyEntity
				{
					Id = RealtyIds[i],
					CreatedBy = Guid.Parse("1fef01ff-3306-4d4f-a69d-6b4776142ecd"),
					Type = (RealtyType)(i % 3),
					WallType = (WallType)(i % 3),
					HeatingType = (HeatingType)(i % 3),
					Area = 50 + i,
					Floor = (i % 10) + 1,
					Rooms = (i % 5) + 1,
					Baths = (i % 3) + 1,
					Address = new Address
					{
						Street = $"Street {i}",
						City = i % 2 == 0 ? "Kyiv" : "Lviv",
						Region = i % 2 == 0 ? "Kyivska" : "Lvivska",
						ZipCode = i % 2 == 0 ? "01001" : "79000"
					},
					Status = RealtyStatus.Published,
					BuildDate = DateTime.UtcNow.AddYears(-i)
				});
			}

			await collection.InsertManyAsync(items);
		}
	}
}
