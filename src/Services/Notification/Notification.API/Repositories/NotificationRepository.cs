using BuildingBlocks.Configuration;
using BuildingBlocks.Pagination;
using MongoDB.Driver;
using Notification.API.Models.Entities;
using Notification.API.Models.Filters;

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

		public async Task CreateAsync(NotificationEntity notification, CancellationToken cancellationToken)
		{
			await _collection.InsertOneAsync(notification, null, cancellationToken);
		}

		public async Task<PaginatedResult<NotificationEntity>> GetPaginatedAsync(NotificationFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var builder = Builders<NotificationEntity>.Filter;
			var filter = BuildFilterDefinition(filters);;

			var total = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
			var items = await _collection.Find(filter)
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
			var filter = Builders<NotificationEntity>.Filter.And(
				Builders<NotificationEntity>.Filter.Eq(x => x.UserId, userId),
				Builders<NotificationEntity>.Filter.Eq(x => x.IsRead, false)
			);
			var update = Builders<NotificationEntity>.Update.Set(x => x.IsRead, true);
			var result = await _collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
			return (int)result.ModifiedCount;
		}

		private static FilterDefinition<NotificationEntity> BuildFilterDefinition(NotificationFilters filters)
		{
			var builder = Builders<NotificationEntity>.Filter;
			var filter = builder.Empty;

			if (filters.UserId.HasValue)
				filter &= builder.Eq(x => x.UserId, filters.UserId.Value);
			if (filters.IsRead.HasValue)
				filter &= builder.Eq(x => x.IsRead, filters.IsRead.Value);

			return filter;
		}
	}
}
