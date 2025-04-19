using AutoMapper;
using BuildingBlocks.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Photo.API.Models;
using Photo.API.Models.DTOs.Requests;
using Photo.API.Models.DTOs.Responses;
using Photo.API.Services;
using Photo.API.Services.Interfaces;

namespace Photo.API.Controllers
{
	public class RealtyPhotoController : BaseApiController
	{
		private readonly IRealtyPhotoService _realtyPhotoService;
		private readonly IFileValidatorService _fileValidatorService;
		private readonly IMapper _mapper;

		public RealtyPhotoController(IRealtyPhotoService realtyPhotoService, IFileValidatorService fileValidatorService, IMapper mapper)
		{
			_realtyPhotoService = realtyPhotoService;
			_fileValidatorService = fileValidatorService;
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
			if (request.File == null || request.File.Length == 0)
			{
				throw new BadRequestException("No file uploaded.");
			}

			if (!_fileValidatorService.IsValidExtension(request.File.FileName)
				|| !_fileValidatorService.IsValidMimeType(request.File.ContentType)
				|| !_fileValidatorService.IsValidFile(request.File))
			{
				throw new BadRequestException("Invalid image format.");
			}

			var filePath = await _realtyPhotoService.SaveFileAsync(request.File, "realty", cancellationToken);

			var metadata = new RealtyPhotoMetadata
			{
				RealtyId = request.RealtyId,
				FileName = request.File.FileName,
				ContentType = request.File.ContentType,
				Url = filePath,
			};

			await _realtyPhotoService.AddPhotoAsync(metadata, cancellationToken);

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
	}
}
