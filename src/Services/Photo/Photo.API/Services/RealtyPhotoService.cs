using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Identity;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Pagination;
using Photo.API.Models;
using Photo.API.Models.DTOs.Requests;
using Photo.API.Models.DTOs.Responses;
using Photo.API.Services.Interfaces;

namespace Photo.API.Services
{
	public class RealtyPhotoService : IGenericService<RealtyPhotoMetadataResponse, CreateRealtyPhotoMetadataRequest, UpdateRealtyPhotoMetadataRequest, RealtyPhotoFilters>
	{
		private readonly IGenericRepository<RealtyPhotoMetadata, RealtyPhotoFilters> _repository;
		private readonly IUserIdentityProvider _userIdentityProvider;
		private readonly IFileStorageService _fileStorageService;
		private readonly IFileValidatorService _fileValidatorService;
		private readonly IMapper _mapper;

		public RealtyPhotoService(
			IGenericRepository<RealtyPhotoMetadata, RealtyPhotoFilters> repository,
			IUserIdentityProvider userIdentityProvider,
			IFileStorageService fileStorageService,
			IFileValidatorService fileValidatorService,
			IMapper mapper)
		{
			_repository = repository;
			_userIdentityProvider = userIdentityProvider;
			_fileStorageService = fileStorageService;
			_fileValidatorService = fileValidatorService;
			_mapper = mapper;
		}

		public async Task<RealtyPhotoMetadataResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var entity = await _repository.GetByIdAsync(id, cancellationToken);
			return entity is null ? null : _mapper.Map<RealtyPhotoMetadataResponse>(entity);
		}

		public async Task<PaginatedResult<RealtyPhotoMetadataResponse>> GetPaginatedAsync(RealtyPhotoFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var result = await _repository.GetPaginatedAsync(filters, pagination, cancellationToken);
			var mapped = result.Data.Select(_mapper.Map<RealtyPhotoMetadataResponse>);
			return new PaginatedResult<RealtyPhotoMetadataResponse>(pagination.PageIndex, pagination.PageSize, result.Count, mapped);
		}

		public async Task CreateAsync(CreateRealtyPhotoMetadataRequest request, CancellationToken cancellationToken)
		{
			var id = Guid.NewGuid();
			var filePath = await _fileStorageService.SaveFileAsync(id, request.File, "realty", cancellationToken);
			var thumbnailPath = await _fileStorageService.GenerateThumbnailAsync(id, request.File, "realty", cancellationToken);

			var entity = new RealtyPhotoMetadata
			{
				Id = id,
				RealtyId = request.RealtyId,
				FileName = request.File.FileName,
				ContentType = request.File.ContentType,
				Url = filePath,
				ThumbnailUrl = thumbnailPath
			};

			await _repository.CreateAsync(entity, cancellationToken);
		}

		public async Task UpdateAsync(Guid id, UpdateRealtyPhotoMetadataRequest request, CancellationToken cancellationToken)
		{
			var existing = await _repository.GetByIdAsync(id, cancellationToken);
			if (existing == null)
				throw new NotFoundException($"Photo with id {id} not found.");

			if (existing.CreatedBy != _userIdentityProvider.UserId)
				throw new ForbiddenAccessException("You are not the owner of this photo.");

			if (!string.IsNullOrWhiteSpace(existing.Url))
				await _fileStorageService.DeleteFileAsync(existing.Url, cancellationToken);

			if (!string.IsNullOrWhiteSpace(existing.ThumbnailUrl))
				await _fileStorageService.DeleteFileAsync(existing.ThumbnailUrl, cancellationToken);

			existing.FileName = request.FileName ?? existing.FileName;
			existing.ThumbnailUrl = request.ThumbnailUrl ?? existing.ThumbnailUrl;

			await _repository.UpdateAsync(id, existing, cancellationToken);
		}

		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			var existing = await _repository.GetByIdAsync(id, cancellationToken);
			if (existing == null)
				throw new NotFoundException($"Photo with id {id} not found.");

			if (existing.CreatedBy != _userIdentityProvider.UserId)
				throw new ForbiddenAccessException("You are not the owner of this photo.");

			await _fileStorageService.DeleteFileAsync(existing.Url, cancellationToken);

			if (!string.IsNullOrEmpty(existing.ThumbnailUrl))
				await _fileStorageService.DeleteFileAsync(existing.ThumbnailUrl, cancellationToken);

			await _repository.DeleteAsync(id, cancellationToken);
		}
	}
}
