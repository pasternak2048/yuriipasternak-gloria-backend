using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Controllers;
using GLORIA.Contracts.Dtos.Advert;

namespace GLORIA.Advert.API.Controllers
{
	public class AdvertController
		: GenericBaseController<AdvertResponse, AdvertCreateRequest, AdvertUpdateRequest, AdvertFilters>
	{
		public AdvertController(IGenericService<AdvertResponse, AdvertCreateRequest, AdvertUpdateRequest, AdvertFilters> service) : base(service) { }
	}
}
