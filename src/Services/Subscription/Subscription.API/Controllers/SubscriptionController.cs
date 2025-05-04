using BuildingBlocks.Controllers;
using BuildingBlocks.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Subscription.API.Models.DTOs.Requests;
using Subscription.API.Models.DTOs.Responses;
using Subscription.API.Models.Filters;

namespace Subscription.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SubscriptionController
		: GenericBaseController<AdvertSubscriptionResponse, AdvertSubscriptionCreateRequest, AdvertSubscriptionUpdateRequest, AdvertSubscriptionFilters>
	{
		public SubscriptionController(
			IGenericService<AdvertSubscriptionResponse, AdvertSubscriptionCreateRequest, AdvertSubscriptionUpdateRequest, AdvertSubscriptionFilters> service)
			: base(service)
		{
		}
	}
}
