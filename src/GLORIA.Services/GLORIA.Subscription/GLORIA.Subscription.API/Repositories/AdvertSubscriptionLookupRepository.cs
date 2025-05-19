using GLORIA.BuildingBlocks.Configuration;
using GLORIA.Subscription.API.Models.Entities;
using GLORIA.Subscription.API.Repositories.Interfaces;
using MongoDB.Driver;

namespace GLORIA.Subscription.API.Repositories
{
	public class AdvertSubscriptionLookupRepository : IAdvertSubscriptionLookupRepository
	{
		private readonly IMongoCollection<AdvertSubscriptionEntity> _collection;

		public AdvertSubscriptionLookupRepository(IMongoClient mongoClient, MongoSettings settings)
		{
			_collection = mongoClient
				.GetDatabase(settings.DatabaseName)
				.GetCollection<AdvertSubscriptionEntity>("subscriptions_advert");
		}

		public async Task<IReadOnlyCollection<AdvertSubscriptionEntity>> GetAllAsync(CancellationToken cancellationToken)
		{
			return await _collection.Find(Builders<AdvertSubscriptionEntity>.Filter.Empty)
				.ToListAsync(cancellationToken);
		}
	}
}
