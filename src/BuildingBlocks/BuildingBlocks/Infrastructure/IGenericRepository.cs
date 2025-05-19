using Contracts.Dtos.Common;

namespace BuildingBlocks.Infrastructure
{
	public interface IGenericRepository<TEntity, TFilters>
	where TEntity : class
	where TFilters : BaseFilters
	{
		Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

		Task<PaginatedResult<TEntity>> GetPaginatedAsync(TFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken);

		Task CreateAsync(TEntity entity, CancellationToken cancellationToken);

		Task UpdateAsync(Guid id, TEntity updated, CancellationToken cancellationToken);

		Task DeleteAsync(Guid id, CancellationToken cancellationToken);
	}
}
