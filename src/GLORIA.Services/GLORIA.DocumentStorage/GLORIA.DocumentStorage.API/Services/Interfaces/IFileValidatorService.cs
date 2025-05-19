using GLORIA.Contracts.Enums;

namespace GLORIA.DocumentStorage.API.Services.Interfaces
{
	public interface IFileValidatorService
	{
		bool IsValid(IFormFile file, DocumentType documentType);
	}
}
