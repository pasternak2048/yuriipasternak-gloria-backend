using BuildingBlocks.Filtering;
using BuildingBlocks.Pagination;

namespace BuildingBlocks.Infrastructure
{
	public interface IGenericService<TResponse, TCreateRequest, TUpdateRequest, TFilters>
	where TResponse : class
	where TFilters : BaseFilters
	{
		Task<TResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

		Task<PaginatedResult<TResponse>> GetPaginatedAsync(TFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken);

		Task CreateAsync(TCreateRequest request, CancellationToken cancellationToken);

		Task UpdateAsync(Guid id, TUpdateRequest request, CancellationToken cancellationToken);

		Task DeleteAsync(Guid id, CancellationToken cancellationToken);
	}
}
