using Catalog.API.Models.Enums;
using Catalog.API.Models;
using MongoDB.Driver;

namespace Catalog.API.Data
{
	public class RealtyDataSeeder
	{
		private readonly IMongoCollection<Realty> _collection;

		public RealtyDataSeeder(IMongoClient client, BuildingBlocks.Configurations.MongoSettings settings)
		{
			var database = client.GetDatabase(settings.DatabaseName);
			_collection = database.GetCollection<Realty>("realty");
		}

		public async Task SeedAsync()
		{
			if (!await _collection.Find(_ => true).AnyAsync())
			{
				var items = new List<Realty>
				{
					new Realty { CreatedBy = Guid.Parse("1fef01ff-3306-4d4f-a69d-6b4776142ecd"), Type = RealtyType.Apartment, WallType = WallType.Brick, HeatingType = HeatingType.Central, Area = 84.5, Floor = 5, Rooms = 2, Baths = 1, Address = new Address { Street = "Main", City = "Kyiv", Region = "Kyivska", ZipCode = "01001" }, Status = RealtyStatus.Published, BuildDate = DateTime.UtcNow },
					new Realty { CreatedBy = Guid.Parse("1fef01ff-3306-4d4f-a69d-6b4776142ecd"), Type = RealtyType.House, WallType = WallType.Wood, HeatingType = HeatingType.Autonomous, Area = 120, Floor = 1, Rooms = 4, Baths = 2, Address = new Address { Street = "Green", City = "Lviv", Region = "Lvivska", ZipCode = "79000" }, Status = RealtyStatus.PendingApproval, BuildDate = DateTime.UtcNow }
				};

				await _collection.InsertManyAsync(items);
			}
		}
	}
}
