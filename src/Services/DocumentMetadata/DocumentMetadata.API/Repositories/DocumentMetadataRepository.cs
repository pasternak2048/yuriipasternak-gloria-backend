using BuildingBlocks.Configuration;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Pagination;
using DocumentMetadata.API.Models.Entities;
using DocumentMetadata.API.Models.Filters;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DocumentMetadata.API.Repositories
{
	public class DocumentMetadataRepository : IGenericRepository<DocumentMetadataEntity, DocumentMetadataFilters>
	{
		private readonly IMongoCollection<DocumentMetadataEntity> _collection;

		public DocumentMetadataRepository(IMongoClient client, MongoSettings settings)
		{
			var database = client.GetDatabase(settings.DatabaseName);
			_collection = database.GetCollection<DocumentMetadataEntity>("document_metadata");
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
			var filter = BuildFilterDefinition(filters);
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

		private static FilterDefinition<DocumentMetadataEntity> BuildFilterDefinition(DocumentMetadataFilters filters)
		{
			var builder = Builders<DocumentMetadataEntity>.Filter;
			var filter = builder.Empty;

			if (filters.OwnerUserId.HasValue)
				filter &= builder.Eq(x => x.OwnerUserId, filters.OwnerUserId.Value);
			if (filters.DocumentType.HasValue)
				filter &= builder.Eq(x => x.DocumentType, filters.DocumentType.Value);
			if (filters.OwnerObjectId.HasValue)
				filter &= builder.Eq(x => x.OwnerObjectId, filters.OwnerObjectId.Value);
			if (filters.OwnerObjectType.HasValue)
				filter &= builder.Eq(x => x.OwnerObjectType, filters.OwnerObjectType.Value);

			return filter;
		}
	}
}
