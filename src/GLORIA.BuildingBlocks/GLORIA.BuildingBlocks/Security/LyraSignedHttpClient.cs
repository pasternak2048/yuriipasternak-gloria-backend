using LYRA.Client.Constants;
using LYRA.Client.Interfaces;
using System.Text;
using System.Text.Json;

namespace GLORIA.BuildingBlocks.Security
{
    public class LyraSignedHttpClient : ISignedHttpClient
    {
        private readonly HttpClient _http;
        private readonly ILyraCaller _lyra;

        public LyraSignedHttpClient(HttpClient http, ILyraCaller lyra)
        {
            _http = http;
            _lyra = lyra;
        }

        public async Task<TResponse?> SendSignedAsync<TRequest, TResponse>(
            HttpMethod method,
            string path,
            string targetSystem,
            string callerSystem,
            TRequest body,
            CancellationToken cancellationToken = default)
        {
            var payload = JsonSerializer.Serialize(body);

            var metadata = _lyra.GenerateSignedRequest(
                method: method.Method,
                path: path,
                targetSystemName: targetSystem,
                payload: payload,
                callerSystemName: callerSystem
            );

            var request = new HttpRequestMessage(method, path)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };

            var headers = new Dictionary<string, string>
            {
                [LyraHeaderNames.Caller] = metadata.Request.Caller,
                [LyraHeaderNames.Target] = metadata.Request.Target,
                [LyraHeaderNames.Method] = metadata.Request.Method,
                [LyraHeaderNames.Path] = metadata.Request.Path,
                [LyraHeaderNames.Payload] = metadata.Request.Payload ?? string.Empty,
                [LyraHeaderNames.PayloadHash] = metadata.Request.PayloadHash,
                [LyraHeaderNames.Timestamp] = metadata.Request.Timestamp,
                [LyraHeaderNames.Context] = metadata.Request.Context.ToString(),
                [LyraHeaderNames.Signature] = metadata.Signature
            };

            foreach (var (key, value) in headers)
                request.Headers.Add(key, value);

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
