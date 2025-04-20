using Photo.API.Models;
using Photo.API.Models.DTOs.Requests;

namespace Photo.API.Services.Interfaces
{
	public interface IRealtyPhotoService
	{
		Task<IEnumerable<RealtyPhotoMetadata>> GetPhotosAsync(Guid realtyId, CancellationToken cancellationToken);

		Task AddPhotoAsync(RealtyPhotoMetadata metadata, CancellationToken cancellationToken);

		Task RemovePhotosAsync(Guid realtyId, CancellationToken cancellationToken);

		Task<RealtyPhotoMetadata> UploadRealtyPhotoAsync(UploadRealtyPhotoRequest request, CancellationToken cancellationToken);
	}
}
