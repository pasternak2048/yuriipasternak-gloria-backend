﻿using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Configuration;
using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Dtos.Subscription;
using GLORIA.Subscription.API.Models.Entities;
using MongoDB.Driver;

namespace GLORIA.Subscription.API.Repositories
{
	public class AdvertSubscriptionRepository : IGenericRepository<AdvertSubscriptionEntity, AdvertSubscriptionFilters>
	{
		private readonly IMongoCollection<AdvertSubscriptionEntity> _collection;

		public AdvertSubscriptionRepository(IMongoClient client, MongoSettings settings)
		{
			_collection = client
				.GetDatabase(settings.DatabaseName)
				.GetCollection<AdvertSubscriptionEntity>("subscriptions_advert");
		}

		// ---------- ANY ----------
		public async Task<bool> AnyAsync(AdvertSubscriptionFilters filters, CancellationToken cancellationToken)
		{
			var filter = filters.ToFilter<AdvertSubscriptionEntity>();
			return await _collection.Find(filter).AnyAsync(cancellationToken);
		}

		// ---------- GET BY ID ----------
		public async Task<AdvertSubscriptionEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<AdvertSubscriptionEntity>> GetPaginatedAsync(AdvertSubscriptionFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var filter = BuildFilterDefinition(filters);

			var total = await _collection.CountDocumentsAsync(filter, null, cancellationToken);
			var items = await _collection.Find(filter)
				.SortByDescending(r => r.CreatedAt)
				.Skip(pagination.Skip)
				.Limit(pagination.PageSize)
				.ToListAsync(cancellationToken);

			return new PaginatedResult<AdvertSubscriptionEntity>(pagination.PageIndex, pagination.PageSize, total, items);
		}

		// ---------- CREATE ----------
		public async Task CreateAsync(AdvertSubscriptionEntity entity, CancellationToken cancellationToken)
		{
			await _collection.InsertOneAsync(entity, null, cancellationToken);
		}

		// ---------- UPDATE ----------
		public async Task UpdateAsync(Guid id, AdvertSubscriptionEntity updated, CancellationToken cancellationToken)
		{
			await _collection.ReplaceOneAsync(x => x.Id == id, updated, (ReplaceOptions?)null, cancellationToken);
		}

		// ---------- DELETE ----------
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken);

		// ---------- FILTERS ----------
		private static FilterDefinition<AdvertSubscriptionEntity> BuildFilterDefinition(AdvertSubscriptionFilters filters)
		{
			var builder = Builders<AdvertSubscriptionEntity>.Filter;
			var filter = builder.Empty;

			if (filters.UserId.HasValue)
				filter &= builder.Eq(x => x.UserId, filters.UserId.Value);
			if (filters.EventType.HasValue)
				filter &= builder.Eq(x => x.EventType, filters.EventType.Value);
			if (filters.AdvertType.HasValue)
				filter &= builder.Eq(x => x.AdvertType, filters.AdvertType.Value);
			if (!string.IsNullOrEmpty(filters.Title))
				filter &= builder.Regex(x => x.Title, filters.Title);
			if (!string.IsNullOrEmpty(filters.Street))
				filter &= builder.Regex(x => x.Street, filters.Street);
			if (!string.IsNullOrEmpty(filters.City))
				filter &= builder.Regex(x => x.City, filters.City);
			if (!string.IsNullOrEmpty(filters.Region))
				filter &= builder.Regex(x => x.Region, filters.Region);
			if (filters.MinPrice.HasValue)
				filter &= builder.Gte(x => x.MinPrice, filters.MinPrice.Value);
			if (filters.MaxPrice.HasValue)
				filter &= builder.Lte(x => x.MaxPrice, filters.MaxPrice.Value);
			if (filters.Currency.HasValue)
				filter &= builder.Eq(x => x.Currency, filters.Currency.Value);

			return filter;
		}
	}
}
