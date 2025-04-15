using Catalog.API.Models;
using Catalog.API.Models.Enums;
using Catalog.API.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Catalog.API.Repositories
{
	public class CachedRealtyRepository : IRealtyRepository
	{
		private readonly IRealtyRepository _inner;
		private readonly IDistributedCache _cache;
		private readonly ILogger<CachedRealtyRepository> _logger;
		private static readonly DistributedCacheEntryOptions _cacheOptions =
			new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
			};

		public CachedRealtyRepository(
			IRealtyRepository inner,
			IDistributedCache cache,
			ILogger<CachedRealtyRepository> logger)
		{
			_inner = inner;
			_cache = cache;
			_logger = logger;
		}

		// ---------------- GET ALL ----------------
		public async Task<List<Realty>> GetAllAsync(CancellationToken cancellationToken)
		{
			const string cacheKey = "realty:all";
			var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

			if (!string.IsNullOrEmpty(cached))
			{
				_logger.LogInformation("Retrieved all realties from cache");
				return JsonSerializer.Deserialize<List<Realty>>(cached) ?? [];
			}

			var realties = await _inner.GetAllAsync(cancellationToken);
			await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(realties), _cacheOptions, cancellationToken);
			return realties;
		}

		// ---------------- GET BY ID ----------------
		public async Task<Realty?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var cacheKey = $"realty:{id}";
			var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

			if (!string.IsNullOrEmpty(cached))
			{
				_logger.LogInformation("Retrieved realty {Id} from cache", id);
				return JsonSerializer.Deserialize<Realty>(cached);
			}

			var realty = await _inner.GetByIdAsync(id, cancellationToken);

			if (realty != null)
			{
				await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(realty), _cacheOptions, cancellationToken);
			}

			return realty;
		}

		// ---------------- CREATE ----------------
		public async Task CreateAsync(Realty realty, CancellationToken cancellationToken)
		{
			await _inner.CreateAsync(realty, cancellationToken);

			await _cache.RemoveAsync("realty:all", cancellationToken);
		}

		// ---------------- UPDATE ----------------
		public async Task UpdateAsync(Guid id, Realty updated, CancellationToken cancellationToken)
		{
			await _inner.UpdateAsync(id, updated, cancellationToken);

			var cacheKeyById = $"realty:{id}";
			await _cache.RemoveAsync(cacheKeyById, cancellationToken);

			var allKey = "realty:all";
			await _cache.RemoveAsync(allKey, cancellationToken);
		}

		// ---------------- DELETE ----------------
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			await _inner.DeleteAsync(id, cancellationToken);
			await _cache.RemoveAsync($"realty:{id}", cancellationToken);
			await _cache.RemoveAsync("realty:all", cancellationToken);
		}

		// ---------------- GET FILTERED ----------------
		public async Task<(List<Realty>, long)> GetFilteredAsync(RealtyType? type, RealtyStatus? status, int skip, int take, CancellationToken cancellationToken)
		{
			var cacheKey = $"realty:filtered:type={type?.ToString() ?? "any"}:status={status?.ToString() ?? "any"}:skip={skip}:take={take}";

			var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

			if (!string.IsNullOrEmpty(cached))
			{
				_logger.LogInformation("Retrieved filtered realties from cache");
				return JsonSerializer.Deserialize<FilteredCacheResult>(cached)?.ToTuple() ?? ([], 0);
			}

			var (items, count) = await _inner.GetFilteredAsync(type, status, skip, take, cancellationToken);

			var cacheObject = new FilteredCacheResult { Items = items, TotalCount = count };
			await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(cacheObject), _cacheOptions, cancellationToken);

			return (items, count);
		}

		private class FilteredCacheResult
		{
			public List<Realty> Items { get; set; } = [];
			public long TotalCount { get; set; }

			public (List<Realty>, long) ToTuple() => (Items, TotalCount);
		}
	}
}
