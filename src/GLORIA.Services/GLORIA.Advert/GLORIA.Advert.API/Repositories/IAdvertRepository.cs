using GLORIA.Advert.API.Models.Entities;
using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.Contracts.Dtos.Advert;

namespace GLORIA.Advert.API.Repositories
{
	public interface IAdvertRepository : IGenericRepository<AdvertEntity, AdvertFilters>
	{
		Task<bool> ExistsActiveOrInactiveAsync(Guid realtyId, CancellationToken cancellationToken);
	}
}
