using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Photo.API.Models.DTOs.Requests;
using Photo.API.Models.DTOs.Responses;
using Photo.API.Services.Interfaces;

namespace Photo.API.Controllers
{
	public class RealtyPhotoController : BaseApiController
	{
		private readonly IRealtyPhotoService _realtyPhotoService;
		private readonly IMapper _mapper;

		public RealtyPhotoController(IRealtyPhotoService realtyPhotoService, IMapper mapper)
		{
			_realtyPhotoService = realtyPhotoService;
			_mapper = mapper;
		}

		[HttpGet("{realtyId}")]
		public async Task<IActionResult> GetPhotos(Guid realtyId, CancellationToken cancellationToken)
		{
			var photos = await _realtyPhotoService.GetPhotosAsync(realtyId, cancellationToken);
			var response = _mapper.Map<IEnumerable<RealtyPhotoMetadataResponse>>(photos);
			return Ok(response);
		}

		[Authorize]
		[HttpPost("upload")]
		public async Task<IActionResult> UploadPhoto([FromForm] UploadRealtyPhotoRequest request, CancellationToken cancellationToken)
		{
			var metadata = await _realtyPhotoService.UploadRealtyPhotoAsync(request, cancellationToken);
			var response = _mapper.Map<RealtyPhotoMetadataResponse>(metadata);
			return CreatedAtAction(nameof(GetPhotos), new { realtyId = request.RealtyId }, response);
		}

		[Authorize]
		[HttpDelete("{realtyId}")]
		public async Task<IActionResult> DeletePhotos(Guid realtyId, CancellationToken cancellationToken)
		{
			await _realtyPhotoService.RemovePhotosAsync(realtyId, cancellationToken);
			return NoContent();
		}

		[Authorize]
		[HttpDelete("photo/{id}")]
		public async Task<IActionResult> DeletePhoto(Guid id, CancellationToken cancellationToken)
		{
			await _realtyPhotoService.RemovePhotoByIdAsync(id, cancellationToken);
			return NoContent();
		}
	}
}
