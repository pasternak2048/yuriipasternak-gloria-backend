namespace GLORIA.BuildingBlocks.Infrastructure.Data.Mongo
{
	public class DatabaseInitializer<T> where T : class
	{
		private readonly MongoCollectionSeeder<T> _seeder;

		public DatabaseInitializer(MongoCollectionSeeder<T> seeder)
		{
			_seeder = seeder;
		}

		public async Task InitializeAsync()
		{
			await _seeder.SeedIfEmptyAsync();
		}
	}
}
