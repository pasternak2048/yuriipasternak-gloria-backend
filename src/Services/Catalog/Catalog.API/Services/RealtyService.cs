using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Identity;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Pagination;
using Catalog.API.Models;
using Catalog.API.Models.DTOs.Requests;
using Catalog.API.Models.DTOs.Responses;

namespace Catalog.API.Services
{
	public class RealtyService : IGenericService<RealtyResponse, RealtyCreateRequest, RealtyUpdateRequest, RealtyFilters>
	{
		private readonly IGenericRepository<Realty, RealtyFilters> _repository;
		private readonly IMapper _mapper;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public RealtyService(IGenericRepository<Realty, RealtyFilters> repository, IMapper mapper, IUserIdentityProvider userIdentityProvider)
		{
			_repository = repository;
			_mapper = mapper;
			_userIdentityProvider = userIdentityProvider;
		}

		public async Task<RealtyResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var entity = await _repository.GetByIdAsync(id, cancellationToken);
			return entity is null ? null : _mapper.Map<RealtyResponse>(entity);
		}

		public async Task<PaginatedResult<RealtyResponse>> GetPaginatedAsync(RealtyFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var result = await _repository.GetPaginatedAsync(filters, pagination, cancellationToken);
			var mapped = result.Data.Select(_mapper.Map<RealtyResponse>);
			return new PaginatedResult<RealtyResponse>(pagination.PageIndex, pagination.PageSize, result.Count, mapped);
		}

		public Task CreateAsync(RealtyCreateRequest request, CancellationToken cancellationToken)
		{
			var entity = _mapper.Map<Realty>(request);
			return _repository.CreateAsync(entity, cancellationToken);
		}

		public async Task UpdateAsync(Guid id, RealtyUpdateRequest request, CancellationToken cancellationToken)
		{
			var updated = _mapper.Map<Realty>(request);
			await _repository.UpdateAsync(id, updated, cancellationToken);
		}

		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			var realty = await _repository.GetByIdAsync(id, cancellationToken);
			if (realty == null) throw new NotFoundException($"Realty with id {id} not found.");
			if (realty.CreatedBy != _userIdentityProvider.UserId) throw new ForbiddenAccessException("You are not the owner.");

			await _repository.DeleteAsync(id, cancellationToken);
		}
	}
}
