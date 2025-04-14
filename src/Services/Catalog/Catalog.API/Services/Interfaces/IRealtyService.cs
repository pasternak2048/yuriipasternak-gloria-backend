using BuildingBlocks.Pagination;
using Catalog.API.Models.DTOs.Requests;
using Catalog.API.Models.DTOs.Responses;

namespace Catalog.API.Services.Interfaces
{
	public interface IRealtyService
	{
		public interface IRealtyService
		{
			Task<List<RealtyResponse>> GetAllAsync(CancellationToken cancellationToken);

			Task<RealtyResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

			Task CreateAsync(CreateRealtyRequest request, CancellationToken cancellationToken);

			Task DeleteAsync(Guid id, CancellationToken cancellationToken);

			Task<PaginatedResult<RealtyResponse>> GetFilteredAsync(GetRealtiesRequest request, CancellationToken cancellationToken);
		}
	}
}
