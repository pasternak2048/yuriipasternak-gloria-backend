using GLORIA.Contracts.Dtos.DocumentStorage;
using GLORIA.Contracts.Enums;

namespace GLORIA.DocumentStorage.API.Services.Interfaces
{
	public interface IDocumentStorageService
	{
		Task<DocumentStorageResponse> SaveFileAsync(Guid fileId, IFormFile file, DocumentType documentType, CancellationToken cancellationToken);

		Task DeleteFileAsync(string relativePath, CancellationToken cancellationToken);
	}
}
