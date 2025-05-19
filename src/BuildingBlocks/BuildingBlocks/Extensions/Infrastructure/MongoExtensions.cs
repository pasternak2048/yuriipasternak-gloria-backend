using BuildingBlocks.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BuildingBlocks.Extensions.Infrastructure
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
