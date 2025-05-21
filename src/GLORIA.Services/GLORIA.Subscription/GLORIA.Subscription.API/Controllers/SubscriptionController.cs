using AutoMapper;
using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Controllers;
using GLORIA.Contracts.Dtos.Subscription;
using GLORIA.Contracts.Events;
using GLORIA.Subscription.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GLORIA.Subscription.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SubscriptionController
		: GenericBaseController<AdvertSubscriptionResponse, AdvertSubscriptionCreateRequest, AdvertSubscriptionUpdateRequest, AdvertSubscriptionFilters>
	{
		private readonly AdvertSubscriptionMatchingService _matchingService;

		public SubscriptionController(
			IGenericService<AdvertSubscriptionResponse, AdvertSubscriptionCreateRequest, AdvertSubscriptionUpdateRequest, AdvertSubscriptionFilters> service,
			AdvertSubscriptionMatchingService matchingService,
			IMapper mapper)
			: base(service)
		{
			_matchingService = matchingService;
		}

		[HttpPost("matching/advert")]
		public async Task<IActionResult> GetMatchingAdvertSubscriptions([FromBody] AdvertCreatedEvent @event, CancellationToken cancellationToken)
		{
			var response = await _matchingService.GetMatchingSubscriptionsAsync(@event, cancellationToken);
			return Ok(response);
		}
	}
}
