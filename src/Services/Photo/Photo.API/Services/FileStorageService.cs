using BuildingBlocks.Exceptions;
using Photo.API.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Photo.API.Services
{
	public class FileStorageService : IFileStorageService
	{
		private readonly string _basePath;

		public FileStorageService(IWebHostEnvironment environment)
		{
			_basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
		}

		public async Task<string> SaveFileAsync(Guid fileId, IFormFile file, string targetFolder, CancellationToken cancellationToken)
		{
			if (file == null || file.Length == 0)
				throw new BadRequestException("File is null or empty.");

			var folderPath = Path.Combine(_basePath, targetFolder);
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			var uniqueFileName = $"{fileId}{Path.GetExtension(file.FileName)}";
			var filePath = Path.Combine(folderPath, uniqueFileName);

			await using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream, cancellationToken);
			}

			return $"/images/{targetFolder}/{uniqueFileName}";
		}

		public async Task<string> GenerateThumbnailAsync(Guid fileId, IFormFile file, string targetFolder, CancellationToken cancellationToken)
		{
			var folderPath = Path.Combine(_basePath, targetFolder, "thumbnails");
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			var thumbnailFileName = $"thumb_{fileId}{Path.GetExtension(file.FileName)}";
			var thumbnailPath = Path.Combine(folderPath, thumbnailFileName);

			using var image = await Image.LoadAsync(file.OpenReadStream(), cancellationToken);
			image.Mutate(x => x.Resize(new ResizeOptions
			{
				Mode = ResizeMode.Crop,
				Size = new Size(200, 200)
			}));
			await image.SaveAsync(thumbnailPath, cancellationToken);

			return $"/images/{targetFolder}/thumbnails/{thumbnailFileName}";
		}
	}
}
