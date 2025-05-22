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
            var result = await _http.PostAsync<AdvertCreatedEvent, IReadOnlyCollection<AdvertSubscriptionMatchingResponse>>(
            "api/subscription/matching/advert",
            @event,
            cancellationToken);

            return result ?? Array.Empty<AdvertSubscriptionMatchingResponse>();
        }
	}
}
