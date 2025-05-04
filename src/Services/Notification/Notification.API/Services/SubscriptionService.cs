using BuildingBlocks.Identity;
using Notification.API.Models.DTOs.Requests;
using Notification.API.Models.Entities;
using Notification.API.Models.Enums;
using Notification.API.Repositories.Interfaces;
using Notification.API.Services.Interfaces;

namespace Notification.API.Services
{
	public class SubscriptionService : ISubscriptionService
	{
		private readonly ISubscriptionRepository _repository;
		private readonly IUserIdentityProvider _userIdentityProvider;

		public SubscriptionService(ISubscriptionRepository repository, IUserIdentityProvider userIdentityProvider)
		{
			_repository = repository;
			_userIdentityProvider = userIdentityProvider;
		}

		public Task CreateAsync(CreateSubscriptionRequest request, CancellationToken cancellationToken)
		{
			var subscription = new NotificationSubscription
			{
				Id = Guid.NewGuid(),
				UserId = _userIdentityProvider.UserId.GetValueOrDefault(),
				EventType = request.EventType,
				FilterJson = request.Filter.GetRawText(),
				CreatedAt = DateTime.UtcNow
			};

			return _repository.CreateAsync(subscription, cancellationToken);
		}

		public Task<List<NotificationSubscription>> GetByEventTypeAsync(NotificationEventType eventType, CancellationToken cancellationToken)
		{
			return _repository.GetByEventTypeAsync(eventType, cancellationToken);
		}
	}
}
