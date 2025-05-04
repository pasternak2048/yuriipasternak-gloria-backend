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
	: GenericBaseController<SubscriptionResponse, SubscriptionCreateRequest, SubscriptionUpdateRequest, SubscriptionFilters>
	{
		public SubscriptionController(
			IGenericService<SubscriptionResponse, SubscriptionCreateRequest, SubscriptionUpdateRequest, SubscriptionFilters> service)
			: base(service)
		{
		}
	}
}
