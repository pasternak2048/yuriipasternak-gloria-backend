using BuildingBlocks.Controllers;
using BuildingBlocks.Infrastructure;
using Catalog.API.Models.DTOs.Requests;
using Catalog.API.Models.DTOs.Responses;
using Catalog.API.Models.Filters;

namespace Catalog.API.Controllers
{
	public class RealtyController
		: GenericBaseController<RealtyResponse, RealtyCreateRequest, RealtyUpdateRequest, RealtyFilters>
	{
		public RealtyController(IGenericService<RealtyResponse, RealtyCreateRequest, RealtyUpdateRequest, RealtyFilters> service) : base(service) { }
	}
}
