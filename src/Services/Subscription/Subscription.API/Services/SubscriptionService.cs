using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Identity;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Pagination;
using Subscription.API.Models.DTOs.Requests;
using Subscription.API.Models.DTOs.Responses;
using Subscription.API.Models.Entities;
using Subscription.API.Models.Filters;

namespace Subscription.API.Services
{
	public class SubscriptionService : IGenericService<
		SubscriptionResponse,
		SubscriptionCreateRequest,
		SubscriptionUpdateRequest,
		SubscriptionFilters>
	{
		private readonly IGenericRepository<SubscriptionEntity, SubscriptionFilters> _repository;
		private readonly IMapper _mapper;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public SubscriptionService(
			IGenericRepository<SubscriptionEntity, SubscriptionFilters> repository,
			IMapper mapper,
			IUserIdentityProvider userIdentityProvider)
		{
			_repository = repository;
			_mapper = mapper;
			_userIdentityProvider = userIdentityProvider;
		}

		public async Task<SubscriptionResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var entity = await _repository.GetByIdAsync(id, cancellationToken);
			return entity is null ? null : _mapper.Map<SubscriptionResponse>(entity);
		}

		public async Task<PaginatedResult<SubscriptionResponse>> GetPaginatedAsync(
			SubscriptionFilters filters,
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
			var mapped = result.Data.Select(_mapper.Map<SubscriptionResponse>);
			return new PaginatedResult<SubscriptionResponse>(pagination.PageIndex, pagination.PageSize, result.Count, mapped);
		}

		public async Task CreateAsync(SubscriptionCreateRequest request, CancellationToken cancellationToken)
		{
			var entity = _mapper.Map<SubscriptionEntity>(request);

			entity.Id = Guid.NewGuid();
			entity.CreatedAt = DateTime.UtcNow;
			entity.CreatedBy = _userIdentityProvider.UserId;
			entity.UserId = _userIdentityProvider.UserId.GetValueOrDefault();

			await _repository.CreateAsync(entity, cancellationToken);
		}

		public async Task UpdateAsync(Guid id, SubscriptionUpdateRequest request, CancellationToken cancellationToken)
		{
			var subscription = await _repository.GetByIdAsync(id, cancellationToken);
			if (subscription == null)
				throw new NotFoundException($"Subscription with id {id} not found.");

			if (subscription.UserId != _userIdentityProvider.UserId && !_userIdentityProvider.IsAdmin)
				throw new ForbiddenAccessException("You are not the owner.");

			var updated = _mapper.Map<SubscriptionEntity>(request);
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
