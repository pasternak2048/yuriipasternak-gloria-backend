using Catalog.API.Configurations;
using Catalog.API.Models.Enums;
using Catalog.API.Models;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
	public class RealtyRepository : IRealtyRepository
	{
		private readonly IMongoCollection<Realty> _collection;

		public RealtyRepository(IMongoClient client, MongoSettings settings)
		{
			var database = client.GetDatabase(settings.DatabaseName);
			_collection = database.GetCollection<Realty>("realty");
		}

		public async Task<List<Realty>> GetAllAsync(CancellationToken cancellationToken) => await _collection.Find(_ => true).ToListAsync(cancellationToken);
		public async Task<Realty?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => await _collection.Find(r => r.Id == id).FirstOrDefaultAsync(cancellationToken);
		public async Task CreateAsync(Realty realty, CancellationToken cancellationToken) => await _collection.InsertOneAsync(realty, null, cancellationToken);
		public async Task UpdateAsync(Guid id, Realty updated, CancellationToken cancellationToken) => await _collection.ReplaceOneAsync(r => r.Id == id, updated, (ReplaceOptions?)null, cancellationToken);
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken) => await _collection.DeleteOneAsync(r => r.Id == id, cancellationToken);

		public async Task<(List<Realty> items, long count)> GetFilteredAsync(RealtyType? type, RealtyStatus? status, int skip, int take, CancellationToken cancellationToken)
		{
			var builder = Builders<Realty>.Filter;
			var filter = builder.Empty;

			if (type.HasValue)
				filter &= builder.Eq(r => r.Type, type);

			if (status.HasValue)
				filter &= builder.Eq(r => r.Status, status);

			var count = await _collection.CountDocumentsAsync(filter);
			var items = await _collection.Find(filter).Skip(skip).Limit(take).ToListAsync(cancellationToken);

			return (items, count);
		}
	}
}
