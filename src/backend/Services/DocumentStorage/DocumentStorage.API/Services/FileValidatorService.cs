using BuildingBlocks.Common.Enums;
using DocumentStorage.API.Services.Interfaces;

namespace DocumentStorage.API.Services
{
	public class FileValidatorService : IFileValidatorService
	{
		private static readonly Dictionary<DocumentType, HashSet<string>> AllowedExtensions = new()
		{
			[DocumentType.RealtyImage] = new() { ".jpg", ".jpeg", ".png", ".webp", ".bmp" },
			[DocumentType.ViewImage] = new() { ".jpg", ".jpeg", ".png", ".webp", ".bmp" },
			[DocumentType.UserAvatar] = new() { ".jpg", ".jpeg", ".png", ".webp", ".bmp" },
			[DocumentType.ContractDocument] = new() { ".pdf" }
		};

		private static readonly Dictionary<DocumentType, HashSet<string>> AllowedMimeTypes = new()
		{
			[DocumentType.RealtyImage] = new() { "image/jpeg", "image/png", "image/webp", "image/bmp" },
			[DocumentType.ViewImage] = new() { "image/jpeg", "image/png", "image/webp", "image/bmp" },
			[DocumentType.UserAvatar] = new() { "image/jpeg", "image/png", "image/webp", "image/bmp" },
			[DocumentType.ContractDocument] = new() { "application/pdf" }
		};

		private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB

		public bool IsValid(IFormFile file, DocumentType documentType)
		{
			if (file == null || file.Length == 0 || file.Length > MaxFileSizeBytes)
				return false;

			var extension = Path.GetExtension(file.FileName);
			var contentType = file.ContentType;

			if (!AllowedExtensions.TryGetValue(documentType, out var extensions) || !extensions.Contains(extension))
				return false;

			if (!AllowedMimeTypes.TryGetValue(documentType, out var mimeTypes) || !mimeTypes.Contains(contentType))
				return false;

			return true;
		}
	}
}
