using Catalog.API.Services.Interfaces;

namespace Catalog.API.Services
{
	public class FileValidatorService : IFileValidatorService
	{
		private static readonly Dictionary<string, string> _allowedImageTypes = new()
	{
		{ ".jpg", "image/jpeg" },
		{ ".jpeg", "image/jpeg" },
		{ ".png", "image/png" },
		{ ".gif", "image/gif" },
		{ ".webp", "image/webp" }
	};

		public bool IsValidImage(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return false;

			var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
			if (!_allowedImageTypes.TryGetValue(ext, out var expectedMime))
				return false;

			return file.ContentType.Equals(expectedMime, StringComparison.OrdinalIgnoreCase);
		}
	}
}
