using GLORIA.BuildingBlocks.Security;
using GLORIA.Contracts.Dtos.Subscription;
using GLORIA.Contracts.Events;

namespace GLORIA.Notification.API.ExternalServices.Subscription
{
    public class SubscriptionClient
    {
        private readonly ISignedHttpClient _http;

        public SubscriptionClient(ISignedHttpClient http)
        {
            _http = http;
        }

        public async Task<IReadOnlyCollection<AdvertSubscriptionMatchingResponse>> GetMatchingSubscriptionsAsync(
            AdvertCreatedEvent @event,
            CancellationToken cancellationToken)
        {
            var result = await _http.SendSignedAsync<AdvertCreatedEvent, IReadOnlyCollection<AdvertSubscriptionMatchingResponse>>(
                method: HttpMethod.Post,
                path: "/api/subscription/matching/advert",
                targetSystem: "subscription@gloria",
                callerSystem: "notification@gloria",
                body: @event,
                cancellationToken: cancellationToken);

            return result ?? Array.Empty<AdvertSubscriptionMatchingResponse>();
        }
    }
}
