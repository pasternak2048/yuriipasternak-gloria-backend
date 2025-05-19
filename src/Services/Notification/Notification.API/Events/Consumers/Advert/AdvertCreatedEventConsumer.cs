using Contracts.Dtos.Notification;
using Contracts.Enums;
using Contracts.Events;
using MassTransit;
using Notification.API.ExternalServices.Subscription;
using Notification.API.Services.Interfaces;

namespace Notification.API.Events.Consumers.Advert
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
					Title = $"New advert in {@event.City} — {@event.Title}",
					Message = $"Price: {@event.Price} {@event.Currency}"
				};

				await _notificationService.CreateAsync(notification, context.CancellationToken);
			}
		}
	}
}
