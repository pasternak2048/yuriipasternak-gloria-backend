using BuildingBlocks.Exceptions;
using Photo.API.Models;
using Photo.API.Repositories.Interfaces;
using Photo.API.Services.Interfaces;

namespace Photo.API.Services
{
	public class RealtyPhotoService : IRealtyPhotoService
	{
		private readonly IRealtyPhotoRepository _repository;
		private readonly IUserContextService _userContextService;
		private readonly IFileStorageService _fileStorageService;

		public RealtyPhotoService(IRealtyPhotoRepository repository, IUserContextService userContextService, IFileStorageService fileStorageService)
		{
			_repository = repository;
			_userContextService = userContextService;
			_fileStorageService = fileStorageService;
		}

		public async Task<IEnumerable<RealtyPhotoMetadata>> GetPhotosAsync(Guid realtyId, CancellationToken cancellationToken)
		{
			return await _repository.GetByRealtyIdAsync(realtyId, cancellationToken);
		}

		public async Task AddPhotoAsync(RealtyPhotoMetadata metadata, CancellationToken cancellationToken)
		{
			await _repository.AddAsync(metadata, cancellationToken);
		}

		public async Task RemovePhotosAsync(Guid realtyId, CancellationToken cancellationToken)
		{
			var photos = await _repository.GetByRealtyIdAsync(realtyId, cancellationToken);
			if (photos is null)
			{
				throw new NotFoundException($"Photos with realty id {realtyId} not found.");
			}

			var currentUserId = _userContextService.GetUserId();

			if (!photos.All(p => p.CreatedBy == currentUserId))
			{
				throw new ForbiddenAccessException("You are not the owner of these photos.");
			}

			await _repository.DeleteByRealtyIdAsync(realtyId, cancellationToken);
		}

		public async Task<string> SaveFileAsync(IFormFile file, string targetFolder, CancellationToken cancellationToken)
		{
			return await _fileStorageService.SaveFileAsync(file, targetFolder, cancellationToken);
		}
	}
}
