using BuildingBlocks.Configuration;
using BuildingBlocks.Identity;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Pagination;
using MongoDB.Driver;
using Subscription.API.Models.Entities;
using Subscription.API.Models.Filters;

namespace Subscription.API.Repositories
{
	public class SubscriptionRepository : IGenericRepository<SubscriptionEntity, SubscriptionFilters>
	{
		private readonly IMongoCollection<SubscriptionEntity> _collection;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public SubscriptionRepository(IMongoClient client, MongoSettings settings, IUserIdentityProvider userIdentityProvider)
		{
			_collection = client
				.GetDatabase(settings.DatabaseName)
				.GetCollection<SubscriptionEntity>("subscriptions");

			_userIdentityProvider = userIdentityProvider;
		}

		// ---------- GET BY ID ----------
		public async Task<SubscriptionEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<SubscriptionEntity>> GetPaginatedAsync(SubscriptionFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var filter = BuildFilterDefinition(filters);

			var total = await _collection.CountDocumentsAsync(filter, null, cancellationToken);
			var items = await _collection.Find(filter)
				.Skip(pagination.Skip)
				.Limit(pagination.PageSize)
				.ToListAsync(cancellationToken);

			return new PaginatedResult<SubscriptionEntity>(pagination.PageIndex, pagination.PageSize, total, items);
		}

		// ---------- CREATE ----------
		public async Task CreateAsync(SubscriptionEntity entity, CancellationToken cancellationToken)
		{
			entity.CreatedAt = DateTime.UtcNow;
			entity.CreatedBy = _userIdentityProvider.UserId;

			await _collection.InsertOneAsync(entity, null, cancellationToken);
		}

		// ---------- UPDATE ----------
		public async Task UpdateAsync(Guid id, SubscriptionEntity updated, CancellationToken cancellationToken)
		{
			updated.ModifiedAt = DateTime.UtcNow;
			updated.ModifiedBy = _userIdentityProvider.UserId;

			await _collection.ReplaceOneAsync(x => x.Id == id, updated, (ReplaceOptions?)null, cancellationToken);
		}

		// ---------- DELETE ----------
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken);

		// ---------- FILTERS ----------
		private static FilterDefinition<SubscriptionEntity> BuildFilterDefinition(SubscriptionFilters filters)
		{
			var builder = Builders<SubscriptionEntity>.Filter;
			var filter = builder.Empty;

			if (filters.UserId.HasValue)
				filter &= builder.Eq(x => x.UserId, filters.UserId.Value);

			if (filters.EventType.HasValue)
				filter &= builder.Eq(x => x.EventType, filters.EventType.Value);

			if (!string.IsNullOrEmpty(filters.FilterJson))
				filter &= builder.Eq(x => x.FilterJson, filters.FilterJson);

			return filter;
		}
	}
}
