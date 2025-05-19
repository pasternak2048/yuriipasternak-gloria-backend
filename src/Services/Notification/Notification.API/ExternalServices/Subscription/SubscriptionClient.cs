using Contracts.Dtos.Subscription;
using Contracts.Events;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Notification.API.ExternalServices.Subscription
{
	public class SubscriptionClient
	{
		private readonly HttpClient _http;

		private static readonly JsonSerializerOptions _jsonOptions = new()
		{
			PropertyNameCaseInsensitive = true,
			Converters = { new JsonStringEnumConverter() }
		};

		public SubscriptionClient(HttpClient http)
		{
			_http = http;
		}

		public async Task<IReadOnlyCollection<AdvertSubscriptionResponse>> GetMatchingSubscriptionsAsync(
			AdvertCreatedEvent @event,
			CancellationToken cancellationToken)
		{
			var response = await _http.PostAsJsonAsync("api/subscription/matching/advert", @event, cancellationToken);

			if (!response.IsSuccessStatusCode)
				throw new Exception($"Failed to get subscriptions. Status: {response.StatusCode}");

			var content = await response.Content.ReadAsStringAsync(cancellationToken);

			var result = JsonSerializer.Deserialize<IReadOnlyCollection<AdvertSubscriptionResponse>>(content, _jsonOptions);

			return result ?? Array.Empty<AdvertSubscriptionResponse>();
		}
	}
}
