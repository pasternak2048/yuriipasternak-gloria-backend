using Contracts.Events;
using Notification.API.ExternalServices.Subscription.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Notification.API.ExternalServices.Subscription
{
	public class SubscriptionClient
	{
		private readonly HttpClient _http;

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

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
				Converters = { new JsonStringEnumConverter() }
			};

			var result = JsonSerializer.Deserialize<IReadOnlyCollection<AdvertSubscriptionResponse>>(content, options);

			return result ?? Array.Empty<AdvertSubscriptionResponse>();
		}
	}
}
