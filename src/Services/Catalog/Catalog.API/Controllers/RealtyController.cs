using BuildingBlocks.Abstractions;
using BuildingBlocks.Controllers;
using Contracts.Dtos.Catalog;

namespace Catalog.API.Controllers
{
	public class RealtyController
		: GenericBaseController<RealtyResponse, RealtyCreateRequest, RealtyUpdateRequest, RealtyFilters>
	{
		public RealtyController(IGenericService<RealtyResponse, RealtyCreateRequest, RealtyUpdateRequest, RealtyFilters> service) : base(service) { }
	}
}
