using Advert.API.Models.Entities;
using Advert.API.Models.Filters;
using BuildingBlocks.Infrastructure;

namespace Advert.API.Repositories
{
	public interface IAdvertRepository : IGenericRepository<AdvertEntity, AdvertFilters>
	{
		Task<bool> ExistsActiveOrInactiveAsync(Guid realtyId, CancellationToken cancellationToken);
	}
}
