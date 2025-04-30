using BuildingBlocks.Security;
using DocumentStorage.API.Models;
using DocumentStorage.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocumentStorage.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class DocumentStorageController : ControllerBase
	{
		private readonly IFileStorageService _fileStorageService;

		public DocumentStorageController(IFileStorageService fileStorageService)
		{
			_fileStorageService = fileStorageService;
		}

		/// <summary>
		/// Upload a file and (optionally) generate a thumbnail if it's an image.
		/// </summary>
		[ValidateSignature]
		[HttpPost("upload")]
		public async Task<ActionResult<FileStorageResult>> UploadAsync(
			[FromForm] DocumentUploadRequest request,
			CancellationToken cancellationToken)
		{
			if (request.File == null || request.File.Length == 0)
				return BadRequest("File is required.");

			var fileId = Guid.NewGuid();

			var result = await _fileStorageService.SaveFileAsync(fileId, request.File, request.DocumentType, cancellationToken);

			return Ok(result);
		}

		/// <summary>
		/// Delete a file by its relative path.
		/// </summary>
		[ValidateSignature]
		[HttpDelete("delete")]
		public async Task<IActionResult> DeleteAsync([FromQuery] string relativePath, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
				return BadRequest("Relative path is required.");

			await _fileStorageService.DeleteFileAsync(relativePath, cancellationToken);
			return NoContent();
		}
	}
}
