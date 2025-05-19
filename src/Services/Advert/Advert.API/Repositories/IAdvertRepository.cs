using Advert.API.Models.Entities;
using BuildingBlocks.Abstractions;
using Contracts.Dtos.Advert;

namespace Advert.API.Repositories
{
	public interface IAdvertRepository : IGenericRepository<AdvertEntity, AdvertFilters>
	{
		Task<bool> ExistsActiveOrInactiveAsync(Guid realtyId, CancellationToken cancellationToken);
	}
}
