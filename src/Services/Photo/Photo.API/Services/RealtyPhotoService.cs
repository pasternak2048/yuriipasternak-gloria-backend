using BuildingBlocks.Exceptions;
using BuildingBlocks.Identity;
using Photo.API.Models;
using Photo.API.Models.DTOs.Requests;
using Photo.API.Repositories.Interfaces;
using Photo.API.Services.Interfaces;

namespace Photo.API.Services
{
	public class RealtyPhotoService : IRealtyPhotoService
	{
		private readonly IRealtyPhotoRepository _repository;
		private readonly IUserIdentityProvider _userIdentityProvider;
		private readonly IFileStorageService _fileStorageService;
		private readonly IFileValidatorService _fileValidatorService;

		public RealtyPhotoService(
			IRealtyPhotoRepository repository,
			IUserIdentityProvider userIdentityProvider,
			IFileStorageService fileStorageService,
			IFileValidatorService fileValidatorService)
		{
			_repository = repository;
			_userIdentityProvider = userIdentityProvider;
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

			var currentUserId = _userIdentityProvider.UserId;
			if (!photos.All(p => p.CreatedBy == currentUserId))
			{
				throw new ForbiddenAccessException("You are not the owner of these photos.");
			}

			foreach (var photo in photos)
			{
				if (!string.IsNullOrWhiteSpace(photo.Url))
					await _fileStorageService.DeleteFileAsync(photo.Url, cancellationToken);

				if (!string.IsNullOrWhiteSpace(photo.ThumbnailUrl))
					await _fileStorageService.DeleteFileAsync(photo.ThumbnailUrl, cancellationToken);
			}

			await _repository.DeleteByRealtyIdAsync(realtyId, cancellationToken);
		}

		public async Task RemovePhotoByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var photo = await _repository.GetByIdAsync(id, cancellationToken);
			if (photo is null)
				throw new NotFoundException($"Photo with id {id} not found.");

			var currentUserId = _userIdentityProvider.UserId;
			if (photo.CreatedBy != currentUserId)
				throw new ForbiddenAccessException("You are not the owner of this photo.");

			await _fileStorageService.DeleteFileAsync(photo.Url, cancellationToken);

			if (!string.IsNullOrEmpty(photo.ThumbnailUrl))
				await _fileStorageService.DeleteFileAsync(photo.ThumbnailUrl, cancellationToken);

			await _repository.DeleteByIdAsync(id, cancellationToken);
		}

		public async Task<RealtyPhotoMetadata> UploadRealtyPhotoAsync(UploadRealtyPhotoRequest request, CancellationToken cancellationToken)
		{
			if (!_fileValidatorService.IsValid(request.File))
				throw new BadRequestException("Invalid image.");

			var id = Guid.NewGuid();
			var filePath = await _fileStorageService.SaveFileAsync(id, request.File, "realty", cancellationToken);
			var thumbnailPath = await _fileStorageService.GenerateThumbnailAsync(id, request.File, "realty", cancellationToken);

			var metadata = new RealtyPhotoMetadata
			{
				Id = id,
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
