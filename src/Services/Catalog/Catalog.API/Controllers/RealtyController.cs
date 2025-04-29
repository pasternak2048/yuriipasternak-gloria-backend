using BuildingBlocks.Infrastructure;
using Catalog.API.Models;
using Catalog.API.Models.DTOs.Requests;
using Catalog.API.Models.DTOs.Responses;

namespace Catalog.API.Controllers
{
	public class RealtyController
	: GenericBaseController<RealtyResponse, CreateRealtyRequest, UpdateRealtyRequest, RealtyFilters>
	{
		public RealtyController(IGenericService<RealtyResponse, CreateRealtyRequest, UpdateRealtyRequest, RealtyFilters> service) : base(service) { }
	}
}
