using AutoMapper;
using BuildingBlocks.Abstractions;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Identity;
using Contracts.Dtos.Common;
using Contracts.Dtos.Subscription;
using Subscription.API.Models.Entities;

namespace Subscription.API.Services
{
	public class AdvertSubscriptionService : IGenericService<
		AdvertSubscriptionResponse,
		AdvertSubscriptionCreateRequest,
		AdvertSubscriptionUpdateRequest,
		AdvertSubscriptionFilters>
	{
		private readonly IGenericRepository<AdvertSubscriptionEntity, AdvertSubscriptionFilters> _repository;
		private readonly IMapper _mapper;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public AdvertSubscriptionService(
			IGenericRepository<AdvertSubscriptionEntity, AdvertSubscriptionFilters> repository,
			IMapper mapper,
			IUserIdentityProvider userIdentityProvider)
		{
			_repository = repository;
			_mapper = mapper;
			_userIdentityProvider = userIdentityProvider;
		}

		public async Task<AdvertSubscriptionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var entity = await _repository.GetByIdAsync(id, cancellationToken);
			return entity is null ? null : _mapper.Map<AdvertSubscriptionResponse>(entity);
		}

		public async Task<PaginatedResult<AdvertSubscriptionResponse>> GetPaginatedAsync(
			AdvertSubscriptionFilters filters,
			PaginatedRequest pagination,
			CancellationToken cancellationToken)
		{
			if(_userIdentityProvider.UserId == null)
			{
				throw new UnauthorizedException("You are not authorized.");
			}

			if (!_userIdentityProvider.IsAdmin)
			{
				filters.UserId = _userIdentityProvider.UserId;
			}

			var result = await _repository.GetPaginatedAsync(filters, pagination, cancellationToken);
			var mapped = result.Data.Select(_mapper.Map<AdvertSubscriptionResponse>);
			return new PaginatedResult<AdvertSubscriptionResponse>(pagination.PageIndex, pagination.PageSize, result.Count, mapped);
		}

		public async Task CreateAsync(AdvertSubscriptionCreateRequest request, CancellationToken cancellationToken)
		{
			var entity = _mapper.Map<AdvertSubscriptionEntity>(request);

			entity.Id = Guid.NewGuid();
			entity.CreatedAt = DateTime.UtcNow;
			entity.CreatedBy = _userIdentityProvider.UserId.GetValueOrDefault();
			entity.UserId = _userIdentityProvider.UserId.GetValueOrDefault();

			await _repository.CreateAsync(entity, cancellationToken);
		}

		public async Task UpdateAsync(Guid id, AdvertSubscriptionUpdateRequest request, CancellationToken cancellationToken)
		{
			var subscription = await _repository.GetByIdAsync(id, cancellationToken);
			if (subscription == null)
				throw new NotFoundException($"Subscription with id {id} not found.");

			if (subscription.UserId != _userIdentityProvider.UserId && !_userIdentityProvider.IsAdmin)
				throw new ForbiddenAccessException("You are not the owner.");

			var updated = _mapper.Map<AdvertSubscriptionEntity>(request);
			updated.Id = subscription.Id;
			updated.CreatedAt = subscription.CreatedAt;
			updated.CreatedBy = subscription.CreatedBy;

			await _repository.UpdateAsync(id, updated, cancellationToken);
		}

		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			var subscription = await _repository.GetByIdAsync(id, cancellationToken);
			if (subscription == null)
				throw new NotFoundException($"Subscription with id {id} not found.");

			if (subscription.UserId != _userIdentityProvider.UserId && !_userIdentityProvider.IsAdmin)
				throw new ForbiddenAccessException("You are not the owner.");

			await _repository.DeleteAsync(id, cancellationToken);
		}
	}
}
