using DocumentStorage.API.Models;

namespace DocumentStorage.API.Services.Interfaces
{
	public interface IFileValidatorService
	{
		bool IsValid(IFormFile file, DocumentType documentType);
	}
}
