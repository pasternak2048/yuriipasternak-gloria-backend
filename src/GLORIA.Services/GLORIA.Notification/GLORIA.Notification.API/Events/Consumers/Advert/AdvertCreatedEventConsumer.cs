using GLORIA.Contracts.Dtos.Notification;
using GLORIA.Contracts.Enums;
using GLORIA.Contracts.Events;
using GLORIA.Notification.API.ExternalServices.Subscription;
using GLORIA.Notification.API.Services.Interfaces;
using MassTransit;

namespace GLORIA.Notification.API.Events.Consumers.Advert
{
	public class AdvertCreatedEventConsumer : IConsumer<AdvertCreatedEvent>
	{
		private readonly ILogger<AdvertCreatedEventConsumer> _logger;
		private readonly SubscriptionClient _subscriptionClient;
		private readonly INotificationService _notificationService;

		public AdvertCreatedEventConsumer(ILogger<AdvertCreatedEventConsumer> logger, SubscriptionClient subscriptionClient, INotificationService notificationService)
		{
			_logger = logger;
			_subscriptionClient = subscriptionClient;
			_notificationService = notificationService;
		}

		public async Task Consume(ConsumeContext<AdvertCreatedEvent> context)
		{
			var @event = context.Message;

			var subscriptions = await _subscriptionClient.GetMatchingSubscriptionsAsync(@event, context.CancellationToken);

			foreach (var subscription in subscriptions)
			{
				_logger.LogInformation("[NOTIFICATION] Matched subscription for user {UserId} | Advert: {Title}, {Region}, {City}, {Street}, {Price}{Currency}",
					subscription.UserId, @event.Title, @event.Region, @event.City, @event.Street, @event.Price, @event.Currency);

				var notification = new NotificationCreateRequest
				{
					UserId = subscription.UserId,
					EventType = NotificationEventType.AdvertCreated,
					Title = $"New advert in {@event.City}!",
					Message = $"Address: {@event.City}, {@event.Street} street. Price: {@event.Price} {@event.Currency}",
					Object = new NotificationObject
					{
						Id = @event.AdvertId,
						Type = NotificationObjectType.Advert
					}
				};

				await _notificationService.CreateAsync(notification, context.CancellationToken);
            }
		}
	}
}
