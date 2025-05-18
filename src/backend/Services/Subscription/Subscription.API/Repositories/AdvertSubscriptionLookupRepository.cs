using BuildingBlocks.Configuration;
using MongoDB.Driver;
using Subscription.API.Models.Entities;
using Subscription.API.Repositories.Interfaces;

namespace Subscription.API.Repositories
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
