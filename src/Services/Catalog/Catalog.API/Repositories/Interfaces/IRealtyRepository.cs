using Catalog.API.Models.Enums;
using Catalog.API.Models;

namespace Catalog.API.Repositories.Interfaces
{
	public interface IRealtyRepository
	{
		Task<List<Realty>> GetAllAsync(CancellationToken cancellationToken);

		Task<Realty?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

		Task CreateAsync(Realty realty, CancellationToken cancellationToken);

		Task UpdateAsync(Guid id, Realty updated, CancellationToken cancellationToken);

		Task DeleteAsync(Guid id, CancellationToken cancellationToken);

		Task<(List<Realty> items, long count)> GetFilteredAsync(RealtyType? type, RealtyStatus? status, int skip, int take, CancellationToken cancellationToken);
	}
}
