using System.Net.Http.Json;
using System.Text.Json;

namespace GLORIA.BuildingBlocks.Security
{
    public class SignedHttpClient : ISignedHttpClient
    {
        private readonly HttpClient _http;
        private readonly ISignatureService _signatureService;

        public SignedHttpClient(HttpClient http, ISignatureService signatureService)
        {
            _http = http;
            _signatureService = signatureService;
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest body, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = JsonContent.Create(body)
            };

            var signature = _signatureService.Generate();
            var serviceName = _signatureService.ServiceName;

            request.Headers.Add("X-Service-Name", serviceName);
            request.Headers.Add("X-Service-Signature", signature);

            var response = await _http.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new Exception($"Signed request failed: {(int)response.StatusCode} {response.ReasonPhrase} | {message}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<TResponse>(content);
        }
    }
}
