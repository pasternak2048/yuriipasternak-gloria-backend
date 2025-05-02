using MongoDB.Driver;

namespace BuildingBlocks.Persistence.Mongo
{
	public interface ICollectionSeeder<T>
	{
		Task SeedAsync(IMongoCollection<T> collection);
	}
}
