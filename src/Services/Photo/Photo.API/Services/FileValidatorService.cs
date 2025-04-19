using Photo.API.Services.Interfaces;

namespace Photo.API.Services
{
	public class FileValidatorService : IFileValidatorService
	{
		private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
		private static readonly string[] AllowedMimeTypes = ["image/jpeg", "image/png", "image/gif"];

		public bool IsValidExtension(string fileName)
		{
			var extension = Path.GetExtension(fileName).ToLowerInvariant();
			return AllowedExtensions.Contains(extension);
		}

		public bool IsValidMimeType(string contentType)
		{
			return AllowedMimeTypes.Contains(contentType);
		}

		public bool IsValidFile(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return false;

			var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
			var mimeType = file.ContentType;

			return AllowedExtensions.Contains(extension) && AllowedMimeTypes.Contains(mimeType);
		}
	}
}
