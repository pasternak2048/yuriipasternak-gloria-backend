using Photo.API.Models;

namespace Photo.API.Repositories.Interfaces
{
	public interface IRealtyPhotoRepository
	{
		Task<IEnumerable<RealtyPhotoMetadata>> GetByRealtyIdAsync(Guid realtyId, CancellationToken cancellationToken);

		Task AddAsync(RealtyPhotoMetadata metadata, CancellationToken cancellationToken);

		Task DeleteByRealtyIdAsync(Guid realtyId, CancellationToken cancellationToken);
	}
}
