using BuildingBlocks.Configuration;
using BuildingBlocks.Infrastructure;
using Catalog.API.Models.Entities;
using Contracts.Dtos.Catalog;
using Contracts.Dtos.Common;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
	public class RealtyRepository : IGenericRepository<RealtyEntity, RealtyFilters>
	{
		private readonly IMongoCollection<RealtyEntity> _collection;

		public RealtyRepository(IMongoClient client, MongoSettings settings)
		{
			_collection = client.GetDatabase(settings.DatabaseName).GetCollection<RealtyEntity>("realties");
		}

		// ---------- GET BY ID ----------
		public async Task<RealtyEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.Find(r => r.Id == id).FirstOrDefaultAsync(cancellationToken);

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<RealtyEntity>> GetPaginatedAsync(RealtyFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			
			var filter = BuildFilterDefinition(filters);
			var total = await _collection.CountDocumentsAsync(filter, null, cancellationToken);
			var items = await _collection.Find(filter)
				.SortByDescending(r => r.CreatedAt)
				.Skip(pagination.Skip)
				.Limit(pagination.PageSize)
				.ToListAsync(cancellationToken);

			return new PaginatedResult<RealtyEntity>(pagination.PageIndex, pagination.PageSize, total, items);
		}

		// ---------- CREATE ----------
		public async Task CreateAsync(RealtyEntity realty, CancellationToken cancellationToken)
		{
			await _collection.InsertOneAsync(realty, null, cancellationToken);
		}

		// ---------- UPDATE ----------
		public async Task UpdateAsync(Guid id, RealtyEntity updated, CancellationToken cancellationToken)
		{
			await _collection.ReplaceOneAsync(r => r.Id == id, updated, (ReplaceOptions?)null, cancellationToken);
		}

		// ---------- DELETE ----------
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.DeleteOneAsync(r => r.Id == id, cancellationToken);

		private static FilterDefinition<RealtyEntity> BuildFilterDefinition(RealtyFilters filters)
		{
			var builder = Builders<RealtyEntity>.Filter;
			var filter = builder.Empty;

			if (filters.Type.HasValue)
				filter &= builder.Eq(r => r.Type, filters.Type);
			if (filters.Status.HasValue)
				filter &= builder.Eq(r => r.Status, filters.Status);

			return filter;
		}
	}
}
