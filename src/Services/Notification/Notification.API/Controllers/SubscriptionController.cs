using Microsoft.AspNetCore.Mvc;
using Notification.API.Models.DTOs.Requests;
using Notification.API.Services.Interfaces;

namespace Notification.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SubscriptionController : ControllerBase
	{
		private readonly ISubscriptionService _subscriptionService;

		public SubscriptionController(ISubscriptionService subscriptionService)
		{
			_subscriptionService = subscriptionService;
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateSubscriptionRequest request, CancellationToken cancellationToken)
		{
			await _subscriptionService.CreateAsync(request, cancellationToken);
			return Ok();
		}
	}
}
