using Advert.API.Models.Filters;
using BuildingBlocks.Configuration;
using BuildingBlocks.Identity;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Pagination;
using MongoDB.Driver;
using AdvertEntity = Advert.API.Models.Entities.Advert;

namespace Advert.API.Repositories
{
	public class AdvertRepository : IGenericRepository<AdvertEntity, AdvertFilters>
	{
		private readonly IMongoCollection<AdvertEntity> _collection;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public AdvertRepository(IMongoClient client, MongoSettings settings, IUserIdentityProvider userIdentityProvider)
		{
			_collection = client.GetDatabase(settings.DatabaseName).GetCollection<AdvertEntity>("adverts");
			_userIdentityProvider = userIdentityProvider;
		}

		// ---------- GET BY ID ----------
		public async Task<AdvertEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.Find(a => a.Id == id).FirstOrDefaultAsync(cancellationToken);

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<AdvertEntity>> GetPaginatedAsync(AdvertFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var filter = BuildFilterDefinition(filters);
			var total = await _collection.CountDocumentsAsync(filter, null, cancellationToken);
			var items = await _collection.Find(filter)
				.Skip(pagination.Skip)
				.Limit(pagination.PageSize)
				.ToListAsync(cancellationToken);

			return new PaginatedResult<AdvertEntity>(pagination.PageIndex, pagination.PageSize, total, items);
		}

		// ---------- CREATE ----------
		public async Task CreateAsync(AdvertEntity advert, CancellationToken cancellationToken)
		{
			advert.CreatedAt = DateTime.UtcNow;
			advert.CreatedBy = _userIdentityProvider.UserId.GetValueOrDefault();
			await _collection.InsertOneAsync(advert, null, cancellationToken);
		}

		// ---------- UPDATE ----------
		public async Task UpdateAsync(Guid id, AdvertEntity updated, CancellationToken cancellationToken)
		{
			updated.ModifiedAt = DateTime.UtcNow;
			updated.ModifiedBy = _userIdentityProvider.UserId;
			await _collection.ReplaceOneAsync(a => a.Id == id, updated, (ReplaceOptions?)null, cancellationToken);
		}

		// ---------- DELETE ----------
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
			=> await _collection.DeleteOneAsync(a => a.Id == id, cancellationToken);

		private static FilterDefinition<AdvertEntity> BuildFilterDefinition(AdvertFilters filters)
		{
			var builder = Builders<AdvertEntity>.Filter;
			var filter = builder.Empty;

			if (filters.RealtyId.HasValue)
				filter &= builder.Eq(a => a.RealtyId, filters.RealtyId);
			if (filters.AdvertType.HasValue)
				filter &= builder.Eq(a => a.AdvertType, filters.AdvertType);
			if (filters.Status.HasValue)
				filter &= builder.Eq(a => a.Status, filters.Status);
			if (filters.MinPrice.HasValue)
				filter &= builder.Gte(a => a.Price, filters.MinPrice.Value);
			if (filters.MaxPrice.HasValue)
				filter &= builder.Lte(a => a.Price, filters.MaxPrice.Value);
			if (!string.IsNullOrEmpty(filters.Street))
				filter &= builder.Eq(a => a.Address.Street, filters.Street);
			if (!string.IsNullOrEmpty(filters.City))
				filter &= builder.Eq(a => a.Address.City, filters.City);
			if (!string.IsNullOrEmpty(filters.Region))
				filter &= builder.Eq(a => a.Address.Region, filters.Region);
			if (!string.IsNullOrEmpty(filters.ZipCode))
				filter &= builder.Eq(a => a.Address.ZipCode, filters.ZipCode);

			return filter;
		}
	}
}
