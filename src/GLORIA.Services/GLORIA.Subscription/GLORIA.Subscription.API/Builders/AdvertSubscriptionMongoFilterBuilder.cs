using GLORIA.Contracts.Enums;
using GLORIA.Contracts.Events;
using GLORIA.Subscription.API.Models.Entities;
using MongoDB.Driver;

namespace GLORIA.Subscription.API.Builders
{
    public static class AdvertSubscriptionMongoFilterBuilder
    {
        public static FilterDefinition<AdvertSubscriptionEntity> From(AdvertCreatedEvent @event)
        {
            var b = Builders<AdvertSubscriptionEntity>.Filter;
            var filter = b.Empty;

            filter &= b.Eq(x => x.EventType, NotificationEventType.AdvertCreated);
            filter &= b.Eq(x => x.AdvertType, @event.AdvertType);
            filter &= b.Eq(x => x.Currency, @event.Currency);
            filter &= b.Lte(x => x.MinPrice, @event.Price);
            filter &= b.Gte(x => x.MaxPrice, @event.Price);

            if (!string.IsNullOrWhiteSpace(@event.Street))
                filter &= b.Eq(x => x.Street, @event.Street);

            if (!string.IsNullOrWhiteSpace(@event.City))
                filter &= b.Eq(x => x.City, @event.City);

            if (!string.IsNullOrWhiteSpace(@event.Region))
                filter &= b.Eq(x => x.Region, @event.Region);

            return filter;
        }
    }
}
