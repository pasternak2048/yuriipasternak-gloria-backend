using BuildingBlocks.Pagination;
using MongoDB.Driver;
using ViewEntity = View.Domain.Entities.View;

namespace View.Infrastructure.Persistence
{
	public class ViewRepository(IMongoCollection<ViewEntity> collection) : IViewRepository
	{
		public async Task<ViewEntity> CreateAsync(ViewEntity view, CancellationToken cancellationToken = default)
		{
			await collection.InsertOneAsync(view, cancellationToken: cancellationToken);
			return view;
		}

		public async Task<ViewEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			return await collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
		}

		public async Task<PaginatedResult<ViewEntity>> GetPaginatedAsync(PaginatedRequest request, CancellationToken cancellationToken = default)
		{
			var query = collection.AsQueryable();

			var count = await collection.CountDocumentsAsync(FilterDefinition<ViewEntity>.Empty, null, cancellationToken);
			var data = query
				.Skip(request.PageIndex * request.PageSize)
				.Take(request.PageSize)
				.ToList();

			return new PaginatedResult<ViewEntity>(request.PageIndex, request.PageSize, count, data);
		}

		public async Task<PaginatedResult<ViewEntity>> GetByRealtyIdAsync(Guid realtyId, PaginatedRequest request, CancellationToken cancellationToken = default)
		{
			var filter = Builders<ViewEntity>.Filter.Eq(x => x.RealtyId, realtyId);

			var total = await collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

			var data = await collection.Find(filter)
				.Skip(request.PageIndex * request.PageSize)
				.Limit(request.PageSize)
				.ToListAsync(cancellationToken);

			return new PaginatedResult<ViewEntity>(request.PageIndex, request.PageSize, total, data);
		}

		public async Task UpdateAsync(ViewEntity view, CancellationToken cancellationToken = default)
		{
			await collection.ReplaceOneAsync(x => x.Id == view.Id, view, cancellationToken: cancellationToken);
		}

		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
		{
			await collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
		}
	}
}
