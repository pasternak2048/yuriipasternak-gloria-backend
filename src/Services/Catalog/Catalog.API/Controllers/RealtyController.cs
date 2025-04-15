using Catalog.API.Models.DTOs.Requests;
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
		[Authorize]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			var realties = await _realtyService.GetAllAsync(cancellationToken);
			return Ok(realties);
		}

		[HttpGet("{id}")]
		[Authorize]
		public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
		{
			var realty = await _realtyService.GetByIdAsync(id, cancellationToken);
			return realty is null ? NotFound() : Ok(realty);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Create([FromBody] CreateRealtyRequest request, CancellationToken cancellationToken)
		{
			await _realtyService.CreateAsync(request, cancellationToken);
			return CreatedAtAction(nameof(GetAll), null);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
		{
			await _realtyService.DeleteAsync(id, cancellationToken);
			return NoContent();
		}
	}
}
