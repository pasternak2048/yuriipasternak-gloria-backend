using MongoDB.Driver;
using Photo.API.Configurations;

namespace Photo.API.Extensions
{
	public static class MongoExtensions
	{
		public static void AddMongoInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			var settings = configuration.GetSection("MongoSettings").Get<MongoSettings>();
			services.AddSingleton(settings);
			services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.ConnectionString));
		}
	}
}
