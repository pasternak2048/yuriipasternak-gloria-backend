using BuildingBlocks.Common.Enums;
using BuildingBlocks.Exceptions;
using DocumentStorage.API.Models.DTOs.Responses;
using DocumentStorage.API.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DocumentStorage.API.Services
{
	public class DocumentStorageService : IDocumentStorageService
	{
		private readonly string _basePath;
		private readonly IFileValidatorService _validator;

		public DocumentStorageService(IWebHostEnvironment environment, IFileValidatorService validator)
		{
			_basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents");
			_validator = validator;
		}

		public async Task<DocumentStorageResponse> SaveFileAsync(Guid fileId, IFormFile file, DocumentType documentType, CancellationToken cancellationToken)
		{
			if (!_validator.IsValid(file, documentType))
				throw new BadRequestException("Invalid file. It may be too large or have an unsupported format.");

			string subFolder = GetSubFolder(documentType);
			string extension = Path.GetExtension(file.FileName);
			string uniqueFileName = $"{fileId}{extension}";

			string folderPath = Path.Combine(_basePath, subFolder);
			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			string filePath = Path.Combine(folderPath, uniqueFileName);
			await using var stream = new FileStream(filePath, FileMode.Create);
			await file.CopyToAsync(stream, cancellationToken);

			string relativeUrl = $"/documents/{subFolder}/{uniqueFileName}";

			string? thumbnailUrl = null;
			if (IsImage(file.ContentType))
			{
				thumbnailUrl = await GenerateThumbnailAsync(fileId, file, subFolder, cancellationToken);
			}

			return new DocumentStorageResponse
			{
				Url = relativeUrl,
				ThumbnailUrl = thumbnailUrl,
				FileName = file.FileName,
				MimeType = file.ContentType
			};
		}

		public Task DeleteFileAsync(string relativePath, CancellationToken cancellationToken)
		{
			var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.TrimStart('/'));
			if (File.Exists(fullPath))
				File.Delete(fullPath);

			return Task.CompletedTask;
		}

		private async Task<string> GenerateThumbnailAsync(Guid fileId, IFormFile file, string subFolder, CancellationToken cancellationToken)
		{
			var thumbnailFolder = Path.Combine(_basePath, subFolder, "thumbnails");
			if (!Directory.Exists(thumbnailFolder))
				Directory.CreateDirectory(thumbnailFolder);

			var thumbnailFileName = $"thumb_{fileId}{Path.GetExtension(file.FileName)}";
			var thumbnailPath = Path.Combine(thumbnailFolder, thumbnailFileName);

			using var image = await Image.LoadAsync(file.OpenReadStream(), cancellationToken);
			image.Mutate(x => x.Resize(new ResizeOptions
			{
				Mode = ResizeMode.Crop,
				Size = new Size(200, 200)
			}));
			await image.SaveAsync(thumbnailPath, cancellationToken);

			return $"/documents/{subFolder}/thumbnails/{thumbnailFileName}";
		}

		private static string GetSubFolder(DocumentType type) => type switch
		{
			DocumentType.RealtyImage => "images/realty",
			DocumentType.ViewImage => "images/view",
			DocumentType.UserAvatar => "images/users",
			DocumentType.ContractDocument => "contracts",
			_ => "other"
		};

		private static bool IsImage(string contentType) =>
			contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
	}
}
