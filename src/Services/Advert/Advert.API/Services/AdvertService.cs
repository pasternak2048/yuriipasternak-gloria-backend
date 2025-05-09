using Advert.API.Messaging;
using Advert.API.Models.DTOs.Requests;
using Advert.API.Models.DTOs.Responses;
using Advert.API.Models.Entities;
using Advert.API.Models.Filters;
using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Identity;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Pagination;
using Contracts.Events;

namespace Advert.API.Services
{
	public class AdvertService : IGenericService<AdvertResponse, AdvertCreateRequest, AdvertUpdateRequest, AdvertFilters>
	{
		private readonly IGenericRepository<AdvertEntity, AdvertFilters> _repository;
		private readonly IMapper _mapper;
		private readonly IUserIdentityProvider _userIdentityProvider;
		private readonly IAdvertEventPublisher _eventPublisher;

		public AdvertService(IGenericRepository<AdvertEntity, AdvertFilters> repository, IMapper mapper, IUserIdentityProvider userIdentityProvider, IAdvertEventPublisher eventPublisher)
		{
			_repository = repository;
			_mapper = mapper;
			_userIdentityProvider = userIdentityProvider;
			_eventPublisher = eventPublisher;
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

		public async Task CreateAsync(AdvertCreateRequest request, CancellationToken cancellationToken)
		{
			var entity = _mapper.Map<AdvertEntity>(request);

			entity.Id = Guid.NewGuid();
			entity.CreatedAt = DateTime.UtcNow;
			entity.CreatedBy = _userIdentityProvider.UserId.GetValueOrDefault();

			await _repository.CreateAsync(entity, cancellationToken);

			var @event = _mapper.Map<AdvertCreatedEvent>(entity);

			await _eventPublisher.PublishAdvertCreatedAsync(@event);
		}

		public async Task UpdateAsync(Guid id, AdvertUpdateRequest request, CancellationToken cancellationToken)
		{
			var advert = await _repository.GetByIdAsync(id, cancellationToken);
			if (advert == null) throw new NotFoundException($"Advert with id {id} not found.");
			if (advert.CreatedBy != _userIdentityProvider.UserId && !_userIdentityProvider.IsAdmin)
				throw new ForbiddenAccessException("You are not the owner.");

			var updated = _mapper.Map<AdvertEntity>(request);
			updated.ModifiedAt = DateTime.UtcNow;
			updated.ModifiedBy = _userIdentityProvider.UserId.GetValueOrDefault();
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
