using GLORIA.BuildingBlocks.Configuration;
using GLORIA.Contracts.Events;
using GLORIA.Subscription.API.Builders;
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

        public async Task<IReadOnlyCollection<AdvertSubscriptionEntity>> GetMatchingAsync(AdvertCreatedEvent @event, CancellationToken cancellationToken)
        {
            var filter = AdvertSubscriptionMongoFilterBuilder.From(@event);
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }
    }
}
