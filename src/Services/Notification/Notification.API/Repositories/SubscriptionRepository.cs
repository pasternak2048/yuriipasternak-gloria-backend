using BuildingBlocks.Configuration;
using MongoDB.Driver;
using Notification.API.Models.Entities;
using Notification.API.Models.Enums;
using Notification.API.Repositories.Interfaces;

namespace Notification.API.Repositories
{
	public class SubscriptionRepository : ISubscriptionRepository
	{
		private readonly IMongoCollection<NotificationSubscription> _collection;

		public SubscriptionRepository(IMongoClient client, MongoSettings settings)
		{
			var database = client.GetDatabase(settings.DatabaseName);
			_collection = database.GetCollection<NotificationSubscription>("subscriptions");
		}

		public Task CreateAsync(NotificationSubscription subscription, CancellationToken cancellationToken)
		{
			return _collection.InsertOneAsync(subscription, null, cancellationToken);
		}

		public async Task<List<NotificationSubscription>> GetByEventTypeAsync(NotificationEventType eventType, CancellationToken cancellationToken)
		{
			var filter = Builders<NotificationSubscription>.Filter.Eq(s => s.EventType, eventType);
			return await _collection.Find(filter).ToListAsync(cancellationToken);
		}
	}
}
