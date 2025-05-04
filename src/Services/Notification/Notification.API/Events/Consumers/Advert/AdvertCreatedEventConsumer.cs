using Contracts.Events;
using MassTransit;
using Notification.API.Models.Enums;
using Notification.API.Models.Filters.Advert;
using Notification.API.Services.Interfaces;
using System.Text.Json;
using NotificationEntity = Notification.API.Models.Entities.Notification;

namespace Notification.API.Events.Consumers.Advert
{
	public class AdvertCreatedEventConsumer : IConsumer<AdvertCreatedEvent>
	{
		private readonly ILogger<AdvertCreatedEventConsumer> _logger;
		private readonly ISubscriptionService _subscriptionService;
		private readonly INotificationService _notificationService;

		public AdvertCreatedEventConsumer(ILogger<AdvertCreatedEventConsumer> logger, ISubscriptionService subscriptionService, INotificationService notificationService)
		{
			_logger = logger;
			_subscriptionService = subscriptionService;
			_notificationService = notificationService;
		}

		public async Task Consume(ConsumeContext<AdvertCreatedEvent> context)
		{
			var message = context.Message;
			var subscriptions = await _subscriptionService.GetByEventTypeAsync(NotificationEventType.AdvertCreated, context.CancellationToken);

			foreach (var subscription in subscriptions)
			{
				try
				{
					var filter = JsonSerializer.Deserialize<AdvertCreatedFilter>(subscription.FilterJson);
					if (filter is null) continue;

					bool matches =
						(filter.Street == null || filter.City == message.Street) &&
						(filter.City == null || filter.City == message.City) &&
						(filter.Region == null || filter.Region == message.Region) &&
						(filter.AdvertType == null || filter.AdvertType == message.AdvertType) &&
						(filter.Price == null || filter.Price == message.Price) &&
						(filter.Currency == null || filter.Currency == message.Currency);

					if (matches)
					{
						_logger.LogInformation("[NOTIFICATION] Matched subscription for user {UserId} | Advert: {Title}, {Region}, {City}, {Street}, {Price}{Currency}",
							subscription.UserId, message.Title, message.Region, message.City, message.Street, message.Price, message.Currency);

						var notification = new NotificationEntity
						{
							Id = Guid.NewGuid(),
							UserId = subscription.UserId,
							EventType = NotificationEventType.AdvertCreated,
							Title = $"New advert in {message.City} — {message.Title}",
							Message = $"Price: {message.Price} {message.Currency}",
							CreatedAt = DateTime.UtcNow,
							IsRead = false
						};

						await _notificationService.CreateAsync(notification, context.CancellationToken);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Failed to deserialize or match filter for subscription {Id}", subscription.Id);
				}
			}
		}
	}
}
