using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Configuration;
using GLORIA.Catalog.API.Models.Entities;
using GLORIA.Contracts.Dtos.Catalog;
using GLORIA.Contracts.Dtos.Common;
using MongoDB.Driver;

namespace GLORIA.Catalog.API.Repositories
{
	public class RealtyRepository : IGenericRepository<RealtyEntity, RealtyFilters>
	{
		private readonly IMongoCollection<RealtyEntity> _collection;

		public RealtyRepository(IMongoClient client, MongoSettings settings)
		{
			_collection = client.GetDatabase(settings.DatabaseName).GetCollection<RealtyEntity>("realties");
		}

		// ---------- ANY ----------
		public async Task<bool> AnyAsync(RealtyFilters filters, CancellationToken cancellationToken)
		{
			var filter = filters.ToFilter<RealtyEntity>();
			return await _collection.Find(filter).AnyAsync(cancellationToken);
		}

		// ---------- GET BY ID ----------
		public async Task<RealtyEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.Find(r => r.Id == id).FirstOrDefaultAsync(cancellationToken);

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<RealtyEntity>> GetPaginatedAsync(RealtyFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{

			var filter = filters.ToFilter<RealtyEntity>();
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
	}
}
