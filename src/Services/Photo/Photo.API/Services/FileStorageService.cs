using BuildingBlocks.Exceptions;
using Photo.API.Services.Interfaces;

namespace Photo.API.Services
{
	public class FileStorageService : IFileStorageService
	{
		private readonly string _basePath;

		public FileStorageService(IWebHostEnvironment environment)
		{
			_basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
		}

		public async Task<string> SaveFileAsync(IFormFile file, string targetFolder, CancellationToken cancellationToken)
		{
			if (file == null || file.Length == 0)
				throw new BadRequestException("File is null or empty.");

			var folderPath = Path.Combine(_basePath, targetFolder);

			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			var filePath = Path.Combine(folderPath, file.FileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream, cancellationToken);
			}

			return $"/images/{targetFolder}/{file.FileName}";
		}
	}
}
