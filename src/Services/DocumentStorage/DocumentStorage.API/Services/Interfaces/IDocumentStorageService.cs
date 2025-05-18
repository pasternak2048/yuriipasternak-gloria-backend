using BuildingBlocks.Common.Enums;
using DocumentStorage.API.Models.DTOs.Responses;

namespace DocumentStorage.API.Services.Interfaces
{
	public interface IDocumentStorageService
	{
		Task<DocumentStorageResponse> SaveFileAsync(Guid fileId, IFormFile file, DocumentType documentType, CancellationToken cancellationToken);

		Task DeleteFileAsync(string relativePath, CancellationToken cancellationToken);
	}
}
