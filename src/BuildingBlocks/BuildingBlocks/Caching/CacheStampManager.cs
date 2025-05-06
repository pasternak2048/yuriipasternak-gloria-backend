using Microsoft.Extensions.Caching.Distributed;

namespace BuildingBlocks.Caching
{
	public class CacheStampManager
	{
		private readonly IDistributedCache _cache;

		public CacheStampManager(IDistributedCache cache)
		{
			_cache = cache;
		}

		public async Task<string> GetStampAsync(string entityName, CancellationToken cancellationToken)
		{
			var stamp = await _cache.GetStringAsync($"{entityName}:version", cancellationToken);
			return stamp ?? "0";
		}

		public async Task BumpStampAsync(string entityName, CancellationToken cancellationToken)
		{
			var newStamp = DateTime.UtcNow.Ticks.ToString();
			await _cache.SetStringAsync(
				$"{entityName}:version",
				newStamp,
				new DistributedCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
				},
				cancellationToken);
		}
	}
}
