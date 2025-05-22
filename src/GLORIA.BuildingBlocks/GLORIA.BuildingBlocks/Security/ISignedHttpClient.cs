namespace GLORIA.BuildingBlocks.Security
{
    public interface ISignedHttpClient
    {
        Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest body, CancellationToken cancellationToken = default);
    }
}
