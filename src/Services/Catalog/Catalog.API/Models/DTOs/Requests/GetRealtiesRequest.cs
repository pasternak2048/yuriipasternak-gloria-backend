using BuildingBlocks.Pagination;
using Catalog.API.Models.Enums;

namespace Catalog.API.Models.DTOs.Requests
{
	public class GetRealtiesRequest : PaginatedRequest
	{
		public RealtyType? Type { get; set; }

		public RealtyStatus? Status { get; set; }
	}
}
