using GLORIA.Advert.API.Models.Entities;
using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Configuration;
using GLORIA.Contracts.Dtos.Advert;
using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Enums;
using MongoDB.Driver;

namespace GLORIA.Advert.API.Repositories
{
	public class AdvertRepository : IGenericRepository<AdvertEntity, AdvertFilters>
	{
		private readonly IMongoCollection<AdvertEntity> _collection;

		public AdvertRepository(IMongoClient client, MongoSettings settings)
		{
			_collection = client.GetDatabase(settings.DatabaseName).GetCollection<AdvertEntity>("adverts");
		}

		// ---------- ANY ----------
		public async Task<bool> AnyAsync(AdvertFilters filters, CancellationToken cancellationToken)
		{
			var filter = filters.ToFilter<AdvertEntity>();
			return await _collection.Find(filter).AnyAsync(cancellationToken);
		}

		// ---------- GET BY ID ----------
		public async Task<AdvertEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.Find(a => a.Id == id).FirstOrDefaultAsync(cancellationToken);

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<AdvertEntity>> GetPaginatedAsync(AdvertFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var filter = filters.ToFilter<AdvertEntity>();
			var total = await _collection.CountDocumentsAsync(filter, null, cancellationToken);
			var items = await _collection.Find(filter)
				.SortByDescending(r => r.CreatedAt)
				.Skip(pagination.Skip)
				.Limit(pagination.PageSize)
				.ToListAsync(cancellationToken);

			return new PaginatedResult<AdvertEntity>(pagination.PageIndex, pagination.PageSize, total, items);
		}

		// ---------- CREATE ----------
		public async Task CreateAsync(AdvertEntity advert, CancellationToken cancellationToken)
		{
			await _collection.InsertOneAsync(advert, null, cancellationToken);
		}

		// ---------- UPDATE ----------
		public async Task UpdateAsync(Guid id, AdvertEntity updated, CancellationToken cancellationToken)
		{
			await _collection.ReplaceOneAsync(a => a.Id == id, updated, (ReplaceOptions?)null, cancellationToken);
		}

		// ---------- DELETE ----------
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.DeleteOneAsync(a => a.Id == id, cancellationToken);

		public async Task<bool> ExistsActiveOrInactiveAsync(Guid realtyId, CancellationToken cancellationToken)
		{
			var builder = Builders<AdvertEntity>.Filter;
			var filter = builder.Eq(a => a.RealtyId, realtyId) &
						 builder.In(a => a.Status, new[] { AdvertStatus.Active, AdvertStatus.Inactive });

			return await _collection.Find(filter).AnyAsync(cancellationToken);
		}
	}
}
