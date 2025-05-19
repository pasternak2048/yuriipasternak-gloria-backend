using BuildingBlocks.Infrastructure;
using Contracts.Dtos.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class GenericBaseController<TResponse, TCreateRequest, TUpdateRequest, TFilters> : ControllerBase
		where TResponse : class
		where TFilters : BaseFilters
	{
		private readonly IGenericService<TResponse, TCreateRequest, TUpdateRequest, TFilters> _service;

		protected GenericBaseController(IGenericService<TResponse, TCreateRequest, TUpdateRequest, TFilters> service)
		{
			_service = service;
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<TResponse>> GetById(Guid id, CancellationToken cancellationToken)
		{
			var result = await _service.GetByIdAsync(id, cancellationToken);
			return result is null ? NotFound() : Ok(result);
		}

		[HttpGet("paginated")]
		public async Task<ActionResult<PaginatedResult<TResponse>>> GetPaginated(
			[FromQuery] TFilters filters,
			[FromQuery] PaginatedRequest pagination,
			CancellationToken cancellationToken)
		{
			var result = await _service.GetPaginatedAsync(filters, pagination, cancellationToken);
			return Ok(result);
		}

		[HttpPost]
		[Authorize]
		public virtual async Task<IActionResult> Create([FromBody] TCreateRequest request, CancellationToken cancellationToken)
		{
			await _service.CreateAsync(request, cancellationToken);
			return NoContent();
		}

		[HttpPut("{id:guid}")]
		[Authorize]
		public virtual async Task<IActionResult> Update(Guid id, [FromBody] TUpdateRequest request, CancellationToken cancellationToken)
		{
			await _service.UpdateAsync(id, request, cancellationToken);
			return NoContent();
		}

		[HttpDelete("{id:guid}")]
		[Authorize]
		public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
		{
			await _service.DeleteAsync(id, cancellationToken);
			return NoContent();
		}
	}
}
