using Notification.API.Models.Entities;
using Notification.API.Repositories;
using Notification.API.Services.Interfaces;

namespace Notification.API.Services
{
	public class NotificationService : INotificationService
	{
		private readonly INotificationRepository _repository;

		public NotificationService(INotificationRepository repository)
		{
			_repository = repository;
		}

		public Task CreateAsync(NotificationEntity notification, CancellationToken cancellationToken)
		{
			return _repository.CreateAsync(notification, cancellationToken);
		}
	}
}
