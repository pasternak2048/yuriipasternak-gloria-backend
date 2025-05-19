using Contracts.Enums;

namespace DocumentStorage.API.Services.Interfaces
{
	public interface IFileValidatorService
	{
		bool IsValid(IFormFile file, DocumentType documentType);
	}
}
