namespace Photo.API.Services.Interfaces
{
	public interface IFileStorageService
	{
		Task<string> SaveFileAsync(Guid fileId, IFormFile file, string targetFolder, CancellationToken cancellationToken);

		Task<string> GenerateThumbnailAsync(Guid fileId, IFormFile file, string targetFolder, CancellationToken cancellationToken);
	}
}
