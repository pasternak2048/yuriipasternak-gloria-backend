using BuildingBlocks.Configuration;
using BuildingBlocks.Identity;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Pagination;
using Catalog.API.Models.Entities;
using Catalog.API.Models.Filters;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
	public class RealtyRepository : IGenericRepository<Realty, RealtyFilters>
	{
		private readonly IMongoCollection<Realty> _collection;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public RealtyRepository(IMongoClient client, MongoSettings settings, IUserIdentityProvider userIdentityProvider)
		{
			_collection = client.GetDatabase(settings.DatabaseName).GetCollection<Realty>("realties");
			_userIdentityProvider = userIdentityProvider;
		}

		// ---------- GET BY ID ----------
		public async Task<Realty?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.Find(r => r.Id == id).FirstOrDefaultAsync(cancellationToken);

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<Realty>> GetPaginatedAsync(RealtyFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			
			var filter = BuildFilterDefinition(filters);
			var total = await _collection.CountDocumentsAsync(filter, null, cancellationToken);
			var items = await _collection.Find(filter)
				.Skip(pagination.Skip)
				.Limit(pagination.PageSize)
				.ToListAsync(cancellationToken);

			return new PaginatedResult<Realty>(pagination.PageIndex, pagination.PageSize, total, items);
		}

		// ---------- CREATE ----------
		public async Task CreateAsync(Realty realty, CancellationToken cancellationToken)
		{
			realty.CreatedAt = DateTime.UtcNow;
			realty.CreatedBy = _userIdentityProvider.UserId.GetValueOrDefault();
			await _collection.InsertOneAsync(realty, null, cancellationToken);
		}

		// ---------- UPDATE ----------
		public async Task UpdateAsync(Guid id, Realty updated, CancellationToken cancellationToken)
		{
			updated.ModifiedAt = DateTime.UtcNow;
			updated.ModifiedBy = _userIdentityProvider.UserId;
			await _collection.ReplaceOneAsync(r => r.Id == id, updated, (ReplaceOptions?)null, cancellationToken);
		}

		// ---------- DELETE ----------
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.DeleteOneAsync(r => r.Id == id, cancellationToken);

		private static FilterDefinition<Realty> BuildFilterDefinition(RealtyFilters filters)
		{
			var builder = Builders<Realty>.Filter;
			var filter = builder.Empty;

			if (filters.Type.HasValue)
				filter &= builder.Eq(r => r.Type, filters.Type);
			if (filters.Status.HasValue)
				filter &= builder.Eq(r => r.Status, filters.Status);

			return filter;
		}
	}
}
