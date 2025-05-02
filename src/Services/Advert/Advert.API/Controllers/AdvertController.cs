using Advert.API.Models.DTOs.Requests;
using Advert.API.Models.DTOs.Responses;
using Advert.API.Models.Filters;
using BuildingBlocks.Controllers;
using BuildingBlocks.Infrastructure;

namespace Advert.API.Controllers
{
	public class AdvertController
		: GenericBaseController<AdvertResponse, AdvertCreateRequest, AdvertUpdateRequest, AdvertFilters>
	{
		public AdvertController(IGenericService<AdvertResponse, AdvertCreateRequest, AdvertUpdateRequest, AdvertFilters> service) : base(service) { }
	}
}
