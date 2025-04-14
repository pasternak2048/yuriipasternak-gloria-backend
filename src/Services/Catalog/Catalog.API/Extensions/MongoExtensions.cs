using Catalog.API.Configurations;
using Catalog.API.Data;
using Catalog.API.Services;
using MongoDB.Driver;

namespace Catalog.API.Extensions
{
	public static class MongoExtensions
	{
		public static void AddMongoInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			var settings = configuration.GetSection("MongoSettings").Get<MongoSettings>();
			services.AddSingleton(settings);
			services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.ConnectionString));

			services.AddTransient<RealtyDataSeeder>();
			services.AddTransient<DatabaseInitializer>();
		}
	}
}
