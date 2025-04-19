using Photo.API.Models;

namespace Photo.API.Services.Interfaces
{
	public interface IRealtyPhotoService
	{
		Task<IEnumerable<RealtyPhotoMetadata>> GetPhotosAsync(Guid realtyId, CancellationToken cancellationToken);

		Task AddPhotoAsync(RealtyPhotoMetadata metadata, CancellationToken cancellationToken);

		Task RemovePhotosAsync(Guid realtyId, CancellationToken cancellationToken);

		Task<string> SaveFileAsync(IFormFile file, string targetFolder, CancellationToken cancellationToken);
	}
}
