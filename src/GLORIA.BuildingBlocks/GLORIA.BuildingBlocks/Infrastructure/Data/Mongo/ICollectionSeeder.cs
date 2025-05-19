using MongoDB.Driver;

namespace GLORIA.BuildingBlocks.Infrastructure.Data.Mongo
{
	public interface ICollectionSeeder<T>
	{
		Task SeedAsync(IMongoCollection<T> collection);
	}
}
