using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Controllers;
using GLORIA.Contracts.Dtos.Catalog;

namespace GLORIA.Catalog.API.Controllers
{
	public class RealtyController
		: GenericBaseController<RealtyResponse, RealtyCreateRequest, RealtyUpdateRequest, RealtyFilters>
	{
		public RealtyController(IGenericService<RealtyResponse, RealtyCreateRequest, RealtyUpdateRequest, RealtyFilters> service) : base(service) { }
	}
}
