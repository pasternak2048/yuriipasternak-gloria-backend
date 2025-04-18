namespace Catalog.API.Services.Interfaces
{
	public interface IFileValidatorService
	{
		bool IsValidImage(IFormFile file);
	}
}
