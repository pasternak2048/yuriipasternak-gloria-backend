using Photo.API.Models;

namespace Photo.API.Repositories.Interfaces
{
	public interface IRealtyPhotoRepository
	{
		Task<RealtyPhotoMetadata?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

		Task<IEnumerable<RealtyPhotoMetadata>> GetByRealtyIdAsync(Guid realtyId, CancellationToken cancellationToken);

		Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

		Task AddAsync(RealtyPhotoMetadata metadata, CancellationToken cancellationToken);

		Task DeleteByRealtyIdAsync(Guid realtyId, CancellationToken cancellationToken);
	}
}
