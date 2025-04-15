using BuildingBlocks.Pagination;
using Catalog.API.Models.DTOs.Requests;
using Catalog.API.Models.DTOs.Responses;
using Catalog.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
	public class RealtyController : BaseApiController
	{
		private readonly IRealtyService _realtyService;

		public RealtyController(IRealtyService realtyService)
		{
			_realtyService = realtyService;
		}

		[HttpGet]
		public async Task<ActionResult<List<RealtyResponse>>> GetAll(CancellationToken cancellationToken)
		{
			var result = await _realtyService.GetAllAsync(cancellationToken);
			return Ok(result);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<RealtyResponse>> GetById(Guid id, CancellationToken cancellationToken)
		{
			var result = await _realtyService.GetByIdAsync(id, cancellationToken);
			if (result is null)
				return NotFound();

			return Ok(result);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create(CreateRealtyRequest request, CancellationToken cancellationToken)
		{
			await _realtyService.CreateAsync(request, cancellationToken);
			return NoContent();
		}

		[HttpDelete("{id:guid}")]
		[Authorize]
		public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
		{
			await _realtyService.DeleteAsync(id, cancellationToken);
			return NoContent();
		}

		[HttpGet("filtered")]
		public async Task<ActionResult<PaginatedResult<RealtyResponse>>> GetFiltered([FromQuery] GetRealtiesRequest request, CancellationToken cancellationToken)
		{
			var result = await _realtyService.GetFilteredAsync(request, cancellationToken);
			return Ok(result);
		}
	}
}
