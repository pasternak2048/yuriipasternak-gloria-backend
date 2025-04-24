using BuildingBlocks.Configuration;
using MongoDB.Driver;
using Photo.API.Models;
using Photo.API.Repositories.Interfaces;
using Photo.API.Services.Interfaces;

namespace Photo.API.Repositories
{
	public class RealtyPhotoRepository : IRealtyPhotoRepository
	{
		private readonly IMongoCollection<RealtyPhotoMetadata> _collection;
		private readonly IUserContextService _userContextService;

		public RealtyPhotoRepository(IMongoClient client, MongoSettings settings, IUserContextService userContextService)
		{
			var database = client.GetDatabase(settings.DatabaseName);
			_collection = database.GetCollection<RealtyPhotoMetadata>("realty_photos");
			_userContextService = userContextService;
		}

		public async Task<RealtyPhotoMetadata?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<IEnumerable<RealtyPhotoMetadata>> GetByRealtyIdAsync(Guid realtyId, CancellationToken cancellationToken)
		{
			var filter = Builders<RealtyPhotoMetadata>.Filter.Eq(p => p.RealtyId, realtyId);
			return await _collection.Find(filter).ToListAsync(cancellationToken);
		}

		public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			await _collection.DeleteOneAsync(p => p.Id == id, cancellationToken);
		}

		public async Task AddAsync(RealtyPhotoMetadata metadata, CancellationToken cancellationToken)
		{
			metadata.CreatedAt = DateTime.UtcNow;
			metadata.CreatedBy = _userContextService.GetUserId();

			await _collection.InsertOneAsync(metadata, null, cancellationToken);
		}

		public async Task DeleteByRealtyIdAsync(Guid realtyId, CancellationToken cancellationToken)
		{
			var filter = Builders<RealtyPhotoMetadata>.Filter.Eq(p => p.RealtyId, realtyId);
			await _collection.DeleteManyAsync(filter, cancellationToken);
		}
	}
}
