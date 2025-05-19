using AutoMapper;
using BuildingBlocks.Abstractions;
using BuildingBlocks.Controllers;
using Contracts.Dtos.Subscription;
using Contracts.Events;
using Microsoft.AspNetCore.Mvc;
using Subscription.API.Services;

namespace Subscription.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SubscriptionController
		: GenericBaseController<AdvertSubscriptionResponse, AdvertSubscriptionCreateRequest, AdvertSubscriptionUpdateRequest, AdvertSubscriptionFilters>
	{
		private readonly AdvertSubscriptionMatchingService _matchingService;
		private readonly IMapper _mapper;

		public SubscriptionController(
			IGenericService<AdvertSubscriptionResponse, AdvertSubscriptionCreateRequest, AdvertSubscriptionUpdateRequest, AdvertSubscriptionFilters> service,
			AdvertSubscriptionMatchingService matchingService,
			IMapper mapper)
			: base(service)
		{
			_matchingService = matchingService;
			_mapper = mapper;
		}

		[HttpPost("matching/advert")]
		public async Task<IActionResult> GetMatchingAdvertSubscriptions([FromBody] AdvertCreatedEvent @event, CancellationToken cancellationToken)
		{
			var matches = await _matchingService.GetMatchingSubscriptionsAsync(@event, cancellationToken);
			var response = matches.Select(_mapper.Map<AdvertSubscriptionResponse>);
			return Ok(response);
		}
	}
}
