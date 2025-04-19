using Photo.API.Services.Interfaces;

namespace Photo.API.Services
{
	public class FileValidatorService : IFileValidatorService
	{
		private readonly HashSet<string> _allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		".jpg", ".jpeg", ".png", ".webp", ".bmp"
	};

		private readonly HashSet<string> _allowedMimeTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"image/jpeg", "image/png", "image/webp", "image/bmp"
	};

		private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

		public bool IsValid(IFormFile file)
		{
			if (file == null || file.Length == 0 || file.Length > MaxFileSizeBytes)
				return false;

			if (!IsValidExtension(file.FileName))
				return false;

			if (!IsValidMimeType(file.ContentType))
				return false;

			return true;
		}

		public bool IsValidExtension(string fileName)
		{
			var extension = Path.GetExtension(fileName);
			return _allowedExtensions.Contains(extension);
		}

		public bool IsValidMimeType(string contentType)
		{
			return _allowedMimeTypes.Contains(contentType);
		}
	}
}
