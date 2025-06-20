namespace GLORIA.BuildingBlocks.Security
{
    public interface ISignedHttpClient
    {
        Task<TResponse?> SendSignedAsync<TRequest, TResponse>(
            HttpMethod method,
            string path,
            string targetSystem,
            string callerSystem,
            TRequest body,
            CancellationToken cancellationToken = default);
    }
}
