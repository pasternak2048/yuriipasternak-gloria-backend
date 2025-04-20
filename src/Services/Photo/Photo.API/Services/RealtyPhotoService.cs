using BuildingBlocks.Exceptions;
using Photo.API.Models;
using Photo.API.Models.DTOs.Requests;
using Photo.API.Repositories.Interfaces;
using Photo.API.Services.Interfaces;

namespace Photo.API.Services
{
	public class RealtyPhotoService : IRealtyPhotoService
	{
		private readonly IRealtyPhotoRepository _repository;
		private readonly IUserContextService _userContextService;
		private readonly IFileStorageService _fileStorageService;
		private readonly IFileValidatorService _fileValidatorService;

		public RealtyPhotoService(
			IRealtyPhotoRepository repository,
			IUserContextService userContextService,
			IFileStorageService fileStorageService,
			IFileValidatorService fileValidatorService)
		{
			_repository = repository;
			_userContextService = userContextService;
			_fileStorageService = fileStorageService;
			_fileValidatorService = fileValidatorService;
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
			if (photos is null || !photos.Any())
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

		public async Task<RealtyPhotoMetadata> UploadRealtyPhotoAsync(UploadRealtyPhotoRequest request, CancellationToken cancellationToken)
		{
			if (!_fileValidatorService.IsValid(request.File))
				throw new BadRequestException("Invalid image.");

			var fileId = Guid.NewGuid();

			var filePath = await _fileStorageService.SaveFileAsync(fileId, request.File, "realty", cancellationToken);
			var thumbnailPath = await _fileStorageService.GenerateThumbnailAsync(fileId, request.File, "realty", cancellationToken);

			var metadata = new RealtyPhotoMetadata
			{
				RealtyId = request.RealtyId,
				FileName = Path.GetFileName(filePath),
				ContentType = request.File.ContentType,
				Url = filePath,
				ThumbnailUrl = thumbnailPath
			};

			await AddPhotoAsync(metadata, cancellationToken);
			return metadata;
		}
	}
}
