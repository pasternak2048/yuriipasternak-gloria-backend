using BuildingBlocks.Pagination;
using ViewEntity = View.Domain.Entities.View;

namespace View.Infrastructure.Persistence
{
	public interface IViewRepository
	{
		Task<ViewEntity> CreateAsync(ViewEntity view, CancellationToken cancellationToken = default);

		Task<ViewEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

		Task<PaginatedResult<ViewEntity>> GetPaginatedAsync(PaginatedRequest request, CancellationToken cancellationToken = default);

		Task<PaginatedResult<ViewEntity>> GetByRealtyIdAsync(Guid realtyId, PaginatedRequest request, CancellationToken cancellationToken = default);

		Task UpdateAsync(ViewEntity view, CancellationToken cancellationToken = default);

		Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
	}
}
