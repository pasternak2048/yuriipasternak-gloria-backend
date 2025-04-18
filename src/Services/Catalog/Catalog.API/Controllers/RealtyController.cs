using BuildingBlocks.Exceptions;
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
		private readonly IFileValidatorService _fileValidatorService;

		public RealtyController(IRealtyService realtyService, IFileValidatorService fileValidatorService)
		{
			_realtyService = realtyService;
			_fileValidatorService = fileValidatorService;
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

		[HttpPost("{id}/upload")]
		[Authorize]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> UploadPhoto(Guid id, IFormFile file, CancellationToken cancellationToken)
		{
			if (file == null || file.Length == 0)
			{
				throw new BadRequestException("File is empty.");
			}

			if (!_fileValidatorService.IsValidImage(file))
			{
				throw new BadRequestException("Wrong file type or MIME-type.");
			}

			var ext = Path.GetExtension(file.FileName);
			var fileName = $"{Guid.NewGuid()}{ext}";
			var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);
			}

			var filePath = Path.Combine(uploadPath, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream, cancellationToken);
			}

			var realty = await _realtyService.GetByIdAsync(id, cancellationToken);
			if (realty == null)
			{
				throw new NotFoundException("Realty", id);
			}

			realty.PhotoUrl = $"/uploads/{fileName}";
			await _realtyService.UpdatePhotoUrlAsync(id, realty.PhotoUrl!, cancellationToken);

			return Ok(new { photoUrl = realty.PhotoUrl });
		}
	}
}
