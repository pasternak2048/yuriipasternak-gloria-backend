using AutoMapper;
using BuildingBlocks.Abstractions;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Identity;
using Catalog.API.Models.Entities;
using Contracts.Dtos.Catalog;
using Contracts.Dtos.Common;

namespace Catalog.API.Services
{
	public class RealtyService : IGenericService<RealtyResponse, RealtyCreateRequest, RealtyUpdateRequest, RealtyFilters>
	{
		private readonly IGenericRepository<RealtyEntity, RealtyFilters> _repository;
		private readonly IMapper _mapper;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public RealtyService(IGenericRepository<RealtyEntity, RealtyFilters> repository, IMapper mapper, IUserIdentityProvider userIdentityProvider)
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
			var entity = _mapper.Map<RealtyEntity>(request);
			entity.CreatedAt = DateTime.UtcNow;
			entity.CreatedBy = _userIdentityProvider.UserId.GetValueOrDefault();

			return _repository.CreateAsync(entity, cancellationToken);
		}

		public async Task UpdateAsync(Guid id, RealtyUpdateRequest request, CancellationToken cancellationToken)
		{
			var realty = await _repository.GetByIdAsync(id, cancellationToken);
			if (realty == null) throw new NotFoundException($"Realty with id {id} not found.");
			if (realty.CreatedBy != _userIdentityProvider.UserId && !_userIdentityProvider.IsAdmin)
				throw new ForbiddenAccessException("You are not the owner.");

			var updated = _mapper.Map<RealtyEntity>(request);
			updated.ModifiedAt = DateTime.UtcNow;
			updated.ModifiedBy = _userIdentityProvider.UserId;

			await _repository.UpdateAsync(id, updated, cancellationToken);
		}

		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			var realty = await _repository.GetByIdAsync(id, cancellationToken);
			if (realty == null) throw new NotFoundException($"Realty with id {id} not found.");
			if (realty.CreatedBy != _userIdentityProvider.UserId && !_userIdentityProvider.IsAdmin)
				throw new ForbiddenAccessException("You are not the owner.");

			await _repository.DeleteAsync(id, cancellationToken);
		}
	}
}
