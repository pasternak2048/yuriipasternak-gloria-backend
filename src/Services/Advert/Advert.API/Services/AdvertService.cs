using Advert.API.Models.DTOs.Requests;
using Advert.API.Models.DTOs.Responses;
using Advert.API.Models.Filters;
using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Identity;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Pagination;
using AdvertEntity = Advert.API.Models.Entities.Advert;

namespace Advert.API.Services
{
	public class AdvertService : IGenericService<AdvertResponse, AdvertCreateRequest, AdvertUpdateRequest, AdvertFilters>
	{
		private readonly IGenericRepository<AdvertEntity, AdvertFilters> _repository;
		private readonly IMapper _mapper;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public AdvertService(IGenericRepository<AdvertEntity, AdvertFilters> repository, IMapper mapper, IUserIdentityProvider userIdentityProvider)
		{
			_repository = repository;
			_mapper = mapper;
			_userIdentityProvider = userIdentityProvider;
		}

		public async Task<AdvertResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var entity = await _repository.GetByIdAsync(id, cancellationToken);
			return entity is null ? null : _mapper.Map<AdvertResponse>(entity);
		}

		public async Task<PaginatedResult<AdvertResponse>> GetPaginatedAsync(AdvertFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var result = await _repository.GetPaginatedAsync(filters, pagination, cancellationToken);
			var mapped = result.Data.Select(_mapper.Map<AdvertResponse>);
			return new PaginatedResult<AdvertResponse>(pagination.PageIndex, pagination.PageSize, result.Count, mapped);
		}

		public Task CreateAsync(AdvertCreateRequest request, CancellationToken cancellationToken)
		{
			var entity = _mapper.Map<AdvertEntity>(request);
			return _repository.CreateAsync(entity, cancellationToken);
		}

		public async Task UpdateAsync(Guid id, AdvertUpdateRequest request, CancellationToken cancellationToken)
		{
			var advert = await _repository.GetByIdAsync(id, cancellationToken);
			if (advert == null) throw new NotFoundException($"Advert with id {id} not found.");
			if (advert.CreatedBy != _userIdentityProvider.UserId && !_userIdentityProvider.IsAdmin)
				throw new ForbiddenAccessException("You are not the owner.");

			var updated = _mapper.Map<AdvertEntity>(request);
			await _repository.UpdateAsync(id, updated, cancellationToken);
		}

		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			var advert = await _repository.GetByIdAsync(id, cancellationToken);
			if (advert == null) throw new NotFoundException($"Advert with id {id} not found.");
			if (advert.CreatedBy != _userIdentityProvider.UserId && !_userIdentityProvider.IsAdmin)
				throw new ForbiddenAccessException("You are not the owner.");

			await _repository.DeleteAsync(id, cancellationToken);
		}
	}
}
