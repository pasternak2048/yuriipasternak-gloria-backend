namespace Photo.API.Services.Interfaces
{
	public interface IFileStorageService
	{
		Task<string> SaveFileAsync(IFormFile file, string targetFolder, CancellationToken cancellationToken);
	}
}
