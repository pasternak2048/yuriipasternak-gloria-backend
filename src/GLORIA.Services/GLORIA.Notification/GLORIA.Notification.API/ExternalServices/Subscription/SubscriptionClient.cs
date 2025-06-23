using GLORIA.Contracts.Dtos.Subscription;
using GLORIA.Contracts.Events;
using LYRA.Client.Interfaces;

namespace GLORIA.Notification.API.ExternalServices.Subscription
{
    public class SubscriptionClient
    {
        private readonly ILyraSignedHttpClient _http;

        public SubscriptionClient(ILyraSignedHttpClient http)
        {
            _http = http;
        }

        public async Task<IReadOnlyCollection<AdvertSubscriptionMatchingResponse>> GetMatchingSubscriptionsAsync(
            AdvertCreatedEvent @event,
            CancellationToken cancellationToken)
        {
            var result = await _http.SendAsync<AdvertCreatedEvent, IReadOnlyCollection<AdvertSubscriptionMatchingResponse>>(
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
