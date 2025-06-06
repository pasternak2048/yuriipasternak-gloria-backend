﻿using AutoMapper;
using GLORIA.BuildingBlocks.Exceptions;
using GLORIA.BuildingBlocks.Identity;
using GLORIA.Contracts.Dtos.Advert;
using GLORIA.Contracts.Dtos.Common;
using GLORIA.Contracts.Dtos.DocumentMetadata;
using GLORIA.Contracts.Dtos.Notification;
using GLORIA.Notification.API.Models.Entities;
using GLORIA.Notification.API.Repositories;
using GLORIA.Notification.API.Services.Interfaces;

namespace GLORIA.Notification.API.Services
{
	public class NotificationService : INotificationService
	{
		private readonly INotificationRepository _repository;
		private readonly IUserIdentityProvider _user;
		private readonly IMapper _mapper;

		public NotificationService(INotificationRepository repository, IUserIdentityProvider user, IMapper mapper)
		{
			_repository = repository;
			_user = user;
			_mapper = mapper;
		}

		public async Task CreateAsync(NotificationCreateRequest request, CancellationToken cancellationToken)
		{
			var entity = _mapper.Map<NotificationEntity>(request);
			entity.CreatedAt = DateTime.UtcNow;

			await _repository.CreateAsync(entity, cancellationToken);
		}

		public async Task<PaginatedResult<NotificationResponse>> GetPaginatedAsync(NotificationFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			if(_user.UserId is null)
			{
				throw new UnauthorizedException("You are not authorized.");
			}

			if (!_user.IsAdmin)
				filters.UserId = _user.UserId;

            var result = await _repository.GetPaginatedAsync(filters, pagination, cancellationToken);
            var mapped = result.Data.Select(_mapper.Map<NotificationResponse>);
            return new PaginatedResult<NotificationResponse>(pagination.PageIndex, pagination.PageSize, result.Count, mapped);

        }

		public async Task<NotificationResponse?> MarkAsReadAsync(Guid id, CancellationToken cancellationToken)
		{
			var notification = await _repository.GetByIdAsync(id, cancellationToken);
			if (notification == null) return null;

			if (notification.UserId != _user.UserId && !_user.IsAdmin)
				throw new ForbiddenAccessException("You are not allowed to mark this notification as read.");

			var result = await _repository.MarkAsReadAsync(id, cancellationToken);

			return _mapper.Map<NotificationResponse>(result);

        }

		public Task<int> MarkAllAsReadAsync(CancellationToken cancellationToken)
		{
			if (_user.UserId is null)
			{
				throw new UnauthorizedException("You are not authorized.");
			}

			return _repository.MarkAllAsReadAsync(_user.UserId.GetValueOrDefault(), cancellationToken);
		}
	}
}
