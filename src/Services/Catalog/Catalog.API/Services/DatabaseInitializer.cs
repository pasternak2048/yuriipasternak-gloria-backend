using Catalog.API.Data;

namespace Catalog.API.Services
{
	public class DatabaseInitializer
	{
		private readonly RealtyDataSeeder _seeder;

		public DatabaseInitializer(RealtyDataSeeder seeder)
		{
			_seeder = seeder;
		}

		public async Task InitializeAsync()
		{
			await _seeder.SeedAsync();
		}
	}
}
