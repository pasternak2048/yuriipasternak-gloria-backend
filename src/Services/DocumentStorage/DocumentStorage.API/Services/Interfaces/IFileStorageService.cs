using DocumentStorage.API.Models;

namespace DocumentStorage.API.Services.Interfaces
{
	public interface IFileStorageService
	{
		Task<FileStorageResult> SaveFileAsync(Guid fileId, IFormFile file, DocumentType documentType, CancellationToken cancellationToken);

		Task DeleteFileAsync(string relativePath, CancellationToken cancellationToken);
	}
}
