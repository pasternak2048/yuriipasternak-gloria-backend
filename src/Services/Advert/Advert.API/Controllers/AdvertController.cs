using BuildingBlocks.Controllers;
using BuildingBlocks.Infrastructure;
using Contracts.Dtos.Advert;

namespace Advert.API.Controllers
{
	public class AdvertController
		: GenericBaseController<AdvertResponse, AdvertCreateRequest, AdvertUpdateRequest, AdvertFilters>
	{
		public AdvertController(IGenericService<AdvertResponse, AdvertCreateRequest, AdvertUpdateRequest, AdvertFilters> service) : base(service) { }
	}
}
