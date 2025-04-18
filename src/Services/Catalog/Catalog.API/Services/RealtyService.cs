using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using Catalog.API.Models;
using Catalog.API.Models.DTOs.Requests;
using Catalog.API.Models.DTOs.Responses;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services.Interfaces;

namespace Catalog.API.Services
{
	public class RealtyService : IRealtyService
	{
		private readonly IRealtyRepository _repository;
		private readonly IMapper _mapper;
		private readonly IUserContextService _userContextService;

		public RealtyService(IRealtyRepository repository, IMapper mapper, IUserContextService userContextService)
		{
			_repository = repository;
			_mapper = mapper;
			_userContextService = userContextService;
		}

		public async Task<List<RealtyResponse>> GetAllAsync(CancellationToken cancellationToken)
		{
			var items = await _repository.GetAllAsync(cancellationToken);
			return items.Select(r => _mapper.Map<RealtyResponse>(r)).ToList();
		}

		public async Task<RealtyResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var entity = await _repository.GetByIdAsync(id, cancellationToken);
			return entity is null ? null : _mapper.Map<RealtyResponse>(entity);
		}

		public async Task CreateAsync(CreateRealtyRequest request, CancellationToken cancellationToken)
		{
			var entity = _mapper.Map<Realty>(request);
			await _repository.CreateAsync(entity, cancellationToken);
		}

		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			var realty = await _repository.GetByIdAsync(id, cancellationToken);
			if (realty is null)
			{
				throw new NotFoundException($"Realty with id {id} not found.");
			}

			var currentUserId = _userContextService.GetUserId();

			if (realty.CreatedBy != currentUserId)
			{
				throw new ForbiddenAccessException("You are not the owner of this realty.");
			}

			await _repository.DeleteAsync(id, cancellationToken);
		}

		public async Task<PaginatedResult<RealtyResponse>> GetFilteredAsync(GetRealtiesRequest request, CancellationToken cancellationToken)
		{
			var (items, count) = await _repository.GetFilteredAsync(request.Type, request.Status, request.PageIndex * request.PageSize, request.PageSize, cancellationToken);
			var mapped = items.Select(r => _mapper.Map<RealtyResponse>(r));
			return new PaginatedResult<RealtyResponse>(request.PageIndex, request.PageSize, count, mapped);
		}

		public async Task UpdatePhotoUrlAsync(Guid id, string photoUrl, CancellationToken cancellationToken)
		{
			var realty = await _repository.GetByIdAsync(id, cancellationToken);
			if (realty == null)
				throw new NotFoundException("Realty not found");

			realty.PhotoUrl = photoUrl;
			await _repository.UpdateAsync(id, realty, cancellationToken);
		}
	}
}
