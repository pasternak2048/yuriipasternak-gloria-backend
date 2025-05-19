using BuildingBlocks.Configuration;
using MongoDB.Driver;

namespace BuildingBlocks.Infrastructure.Data.Mongo
{
	public class MongoCollectionSeeder<T>
	{
		private readonly IMongoCollection<T> _collection;
		private readonly ICollectionSeeder<T> _seeder;

		public MongoCollectionSeeder(IMongoClient client, MongoSettings settings, string collectionName, ICollectionSeeder<T> seeder)
		{
			var database = client.GetDatabase(settings.DatabaseName);
			_collection = database.GetCollection<T>(collectionName);
			_seeder = seeder;
		}

		public async Task SeedIfEmptyAsync()
		{
			var exists = await _collection.Find(_ => true).AnyAsync();
			if (!exists)
			{
				await _seeder.SeedAsync(_collection);
			}
		}
	}
}
