using Advert.API.Models.Entities;
using BuildingBlocks.Persistence.Mongo;
using Contracts.Dtos.Common;
using Contracts.Enums;
using MongoDB.Driver;

namespace Advert.API.Data
{
	public class AdvertSeeder : ICollectionSeeder<AdvertEntity>
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

		public async Task SeedAsync(IMongoCollection<AdvertEntity> collection)
		{
			var random = new Random();
			var items = new List<AdvertEntity>();
			for (int i = 0; i < 50; i++)
			{
				var realtyId = RealtyIds[i % RealtyIds.Count];
				items.Add(new AdvertEntity
				{
					Id = Guid.NewGuid(),
					RealtyId = realtyId,
					AdvertType = (AdvertType)(i % 2),
					Price = 50000 + (i * 1000),
					Currency = CurrencyCode.UAH,
					Title = $"Spacious Property {i}",
					Description = i % 3 == 0 ? $"Spacious property for {((AdvertType)(i % 2)).ToString().ToLower()} in excellent condition." : null,
					Status = AdvertStatus.Active,
					Address = new Address
					{
						Street = $"Street {i}",
						City = i % 2 == 0 ? "Kyiv" : "Lviv",
						Region = i % 2 == 0 ? "Kyivska" : "Lvivska",
						ZipCode = i % 2 == 0 ? "01001" : "79000"
					},
					CreatedAt = DateTime.UtcNow.AddDays(-i),
					CreatedBy = Guid.Parse("1fef01ff-3306-4d4f-a69d-6b4776142ecd")
				});
			}

			await collection.InsertManyAsync(items);
		}
	}
}
