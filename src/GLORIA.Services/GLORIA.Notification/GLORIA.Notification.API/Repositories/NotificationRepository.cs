using GLORIA.BuildingBlocks.Configuration;
using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Dtos.Notification;
using GLORIA.Notification.API.Models.Entities;
using MongoDB.Driver;

namespace GLORIA.Notification.API.Repositories
{
	public class NotificationRepository : INotificationRepository
	{
		private readonly IMongoCollection<NotificationEntity> _collection;

		public NotificationRepository(IMongoClient client, MongoSettings settings)
		{
			var database = client.GetDatabase(settings.DatabaseName);
			_collection = database.GetCollection<NotificationEntity>("notifications");
		}

		public async Task CreateAsync(NotificationEntity notification, CancellationToken cancellationToken)
		{
			await _collection.InsertOneAsync(notification, null, cancellationToken);
		}

		public async Task<bool> AnyAsync(NotificationFilters filters, CancellationToken cancellationToken)
		{
			var filter = filters.ToFilter<NotificationEntity>();
			return await _collection.Find(filter).AnyAsync(cancellationToken);
		}

		public async Task<PaginatedResult<NotificationEntity>> GetPaginatedAsync(NotificationFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var filter = filters.ToFilter<NotificationEntity>();

			var total = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
			var items = await _collection.Find(filter)
				.SortByDescending(r => r.CreatedAt)
				.Skip(pagination.Skip)
				.Limit(pagination.PageSize)
				.ToListAsync(cancellationToken);

			return new PaginatedResult<NotificationEntity>(pagination.PageIndex, pagination.PageSize, total, items);
		}

		public async Task<NotificationEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<NotificationEntity?> MarkAsReadAsync(Guid id, CancellationToken cancellationToken)
		{
			var update = Builders<NotificationEntity>.Update.Set(x => x.IsRead, true);
			return await _collection.FindOneAndUpdateAsync(
				x => x.Id == id,
				update,
				new FindOneAndUpdateOptions<NotificationEntity> { ReturnDocument = ReturnDocument.After },
				cancellationToken
			);
		}

		public async Task<int> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken)
		{
			var filters = new NotificationFilters
			{
				UserId = userId,
				IsRead = false
			};

			var filter = filters.ToFilter<NotificationEntity>();
			var update = Builders<NotificationEntity>.Update.Set(x => x.IsRead, true);
			var result = await _collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);

			return (int)result.ModifiedCount;
		}
	}
}
