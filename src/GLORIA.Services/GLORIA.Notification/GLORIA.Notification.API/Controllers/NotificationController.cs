using GLORIA.BuildingBlocks.Identity;
using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Dtos.Notification;
using GLORIA.Notification.API.Models.Entities;
using GLORIA.Notification.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GLORIA.Notification.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class NotificationController : ControllerBase
	{
		private readonly INotificationService _service;
		

		public NotificationController(INotificationService service, IUserIdentityProvider user)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<ActionResult<PaginatedResult<NotificationEntity>>> GetUserNotifications([FromQuery] NotificationFilters filters, [FromQuery] PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var result = await _service.GetPaginatedAsync(filters, pagination, cancellationToken);
			return Ok(result);
		}

		[HttpPut("{id:guid}/read")]
		public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
		{
			var result = await _service.MarkAsReadAsync(id, cancellationToken);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpPut("read/all")]
		public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
		{
			var modifiedCount = await _service.MarkAllAsReadAsync(cancellationToken);
			return Ok(new { updated = modifiedCount });
		}
	}
}
