using BuildingBlocks.Configuration;
using MongoDB.Driver;
using NotificationEntity = Notification.API.Models.Entities.Notification;

namespace Notification.API.Repositories
{
	public class NotificationRepository : INotificationRepository
	{
		private readonly IMongoCollection<NotificationEntity> _collection;

		public NotificationRepository(IMongoClient client, MongoSettings settings)
		{
			var database = client.GetDatabase(settings.DatabaseName);
			_collection = database.GetCollection<NotificationEntity>("notifications");
		}

		public Task CreateAsync(NotificationEntity notification, CancellationToken cancellationToken)
		{
			return _collection.InsertOneAsync(notification, null, cancellationToken);
		}
	}
}
