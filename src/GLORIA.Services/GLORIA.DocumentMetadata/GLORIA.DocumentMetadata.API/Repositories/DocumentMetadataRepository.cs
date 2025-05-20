using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Configuration;
using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Dtos.DocumentMetadata;
using GLORIA.DocumentMetadata.API.Models.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GLORIA.DocumentMetadata.API.Repositories
{
	public class DocumentMetadataRepository : IGenericRepository<DocumentMetadataEntity, DocumentMetadataFilters>
	{
		private readonly IMongoCollection<DocumentMetadataEntity> _collection;

		public DocumentMetadataRepository(IMongoClient client, MongoSettings settings)
		{
			var database = client.GetDatabase(settings.DatabaseName);
			_collection = database.GetCollection<DocumentMetadataEntity>("document_metadata");
		}

		// ---------- ANY ----------
		public async Task<bool> AnyAsync(DocumentMetadataFilters filters, CancellationToken cancellationToken)
		{
			var filter = filters.ToFilter<DocumentMetadataEntity>();
			return await _collection.Find(filter).AnyAsync(cancellationToken);
		}

		// ---------- GET BY ID ----------
		public async Task<DocumentMetadataEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
		}

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<DocumentMetadataEntity>> GetPaginatedAsync(DocumentMetadataFilters filters,
			PaginatedRequest pagination,
			CancellationToken cancellationToken)
		{
			var filter = filters.ToFilter<DocumentMetadataEntity>();
			var total = await _collection.CountDocumentsAsync(filter, null, cancellationToken);
			var items = await _collection.Find(filter)
				.SortByDescending(r => r.CreatedAt)
				.Skip(pagination.Skip)
				.Limit(pagination.PageSize)
				.ToListAsync(cancellationToken);

			return new PaginatedResult<DocumentMetadataEntity>(pagination.PageIndex, pagination.PageSize, total, items);
		}

		// ---------- CREATE ----------
		public async Task CreateAsync(DocumentMetadataEntity entity, CancellationToken cancellationToken)
		{
			await _collection.InsertOneAsync(entity, null, cancellationToken);
		}

		// ---------- UPDATE ----------
		public async Task UpdateAsync(Guid id, DocumentMetadataEntity updated, CancellationToken cancellationToken)
		{
			await _collection.ReplaceOneAsync(x => x.Id == id, updated, cancellationToken: cancellationToken);
		}

		// ---------- DELETE ----------
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
		}
	}
}
