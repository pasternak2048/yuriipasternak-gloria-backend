using BuildingBlocks.Configuration;
using BuildingBlocks.Identity;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Pagination;
using MongoDB.Driver;
using Photo.API.Models;

namespace Photo.API.Repositories
{
	public class RealtyPhotoRepository : IGenericRepository<RealtyPhotoMetadata, RealtyPhotoFilters>
	{
		private readonly IMongoCollection<RealtyPhotoMetadata> _collection;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public RealtyPhotoRepository(IMongoClient client, MongoSettings settings, IUserIdentityProvider userIdentityProvider)
		{
			var database = client.GetDatabase(settings.DatabaseName);
			_collection = database.GetCollection<RealtyPhotoMetadata>("realty_photos");
			_userIdentityProvider = userIdentityProvider;
		}

		// ---------- GET BY ID ----------
		public async Task<RealtyPhotoMetadata?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
			await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<RealtyPhotoMetadata>> GetPaginatedAsync(RealtyPhotoFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var filter = BuildFilterDefinition(filters);
			var total = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
			var items = await _collection.Find(filter)
				.Skip(pagination.Skip)
				.Limit(pagination.PageSize)
				.ToListAsync(cancellationToken);

			return new PaginatedResult<RealtyPhotoMetadata>(pagination.PageIndex, pagination.PageSize, total, items);
		}

		// ---------- CREATE ----------
		public async Task CreateAsync(RealtyPhotoMetadata entity, CancellationToken cancellationToken)
		{
			entity.CreatedAt = DateTime.UtcNow;
			entity.CreatedBy = _userIdentityProvider.UserId;
			await _collection.InsertOneAsync(entity, null, cancellationToken);
		}

		// ---------- UPDATE ----------
		public async Task UpdateAsync(Guid id, RealtyPhotoMetadata updated, CancellationToken cancellationToken)
		{
			var update = Builders<RealtyPhotoMetadata>.Update
				.Set(x => x.FileName, updated.FileName)
				.Set(x => x.ThumbnailUrl, updated.ThumbnailUrl)
				.Set(x => x.ModifiedAt, DateTime.UtcNow)
				.Set(x => x.ModifiedBy, _userIdentityProvider.UserId);

			await _collection.UpdateOneAsync(x => x.Id == id, update, cancellationToken: cancellationToken);
		}

		// ---------- DELETE ----------
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken) =>
			await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken);

		private static FilterDefinition<RealtyPhotoMetadata> BuildFilterDefinition(RealtyPhotoFilters filters)
		{
			var builder = Builders<RealtyPhotoMetadata>.Filter;
			var filter = builder.Empty;

			if (filters.RealtyId.HasValue)
				filter &= builder.Eq(x => x.RealtyId, filters.RealtyId.Value);

			return filter;
		}
	}
}
