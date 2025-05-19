using MongoDB.Driver;

namespace BuildingBlocks.Infrastructure.Data.Mongo
{
	public interface ICollectionSeeder<T>
	{
		Task SeedAsync(IMongoCollection<T> collection);
	}
}
