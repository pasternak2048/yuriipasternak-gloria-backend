using BuildingBlocks.Pagination;
using ViewEntity = View.Domain.Entities.View;

public interface IViewService
{
	Task<ViewEntity> CreateAsync(ViewEntity view, CancellationToken cancellationToken = default);

	Task<ViewEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

	Task<PaginatedResult<ViewEntity>> GetAllForUserAsync(PaginatedRequest request, CancellationToken cancellationToken = default);

	Task<PaginatedResult<ViewEntity>> GetByRealtyIdForUserAsync(Guid realtyId, PaginatedRequest request, CancellationToken cancellationToken = default);

	Task UpdateAsync(ViewEntity view, CancellationToken cancellationToken = default);

	Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
