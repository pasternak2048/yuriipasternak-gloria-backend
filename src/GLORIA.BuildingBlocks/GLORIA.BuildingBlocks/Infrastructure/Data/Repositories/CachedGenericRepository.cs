using GLORIA.BuildingBlocks.Abstractions;
using GLORIA.BuildingBlocks.Infrastructure.Data.Caching;
using GLORIA.Contracts.Dtos.Common;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GLORIA.BuildingBlocks.Infrastructure.Data.Repositories
{
	public class CachedGenericRepository<T, TFilters> : IGenericRepository<T, TFilters>
	where T : class, IEntity
	where TFilters : BaseFilters
	{
		private readonly IGenericRepository<T, TFilters> _inner;
		private readonly IDistributedCache _cache;
		private readonly CacheStampManager _stampManager;
		private readonly ILogger<CachedGenericRepository<T, TFilters>> _logger;

		private static readonly DistributedCacheEntryOptions CacheOptions = new()
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
		};

		private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

		public CachedGenericRepository(
			IGenericRepository<T, TFilters> inner,
			IDistributedCache cache,
			CacheStampManager stampManager,
			ILogger<CachedGenericRepository<T, TFilters>> logger)
		{
			_inner = inner;
			_cache = cache;
			_stampManager = stampManager;
			_logger = logger;
		}

		// ---------- ANY ----------
		public async Task<bool> AnyAsync(TFilters filters, CancellationToken cancellationToken = default)
		{
			var stamp = await GetStampAsync(cancellationToken);
			var cacheKey = BuildAnyCacheKey(stamp, filters);

			var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
			if (bool.TryParse(cached, out var resultBool))
			{
				_logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
				return resultBool;
			}

			var result = await _inner.AnyAsync(filters, cancellationToken);
			await _cache.SetStringAsync(cacheKey, result.ToString(), CacheOptions, cancellationToken);
			return result;
		}

		// ---------- GET BY ID ----------
		public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var cacheKey = BuildEntityCacheKey(id);
			var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

			if (!string.IsNullOrEmpty(cached))
			{
				try
				{
					var entity = JsonSerializer.Deserialize<T>(cached, SerializerOptions);
					if (entity is not null)
					{
						_logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
						return entity;
					}
				}
				catch (JsonException ex)
				{
					_logger.LogWarning(ex, "Failed to deserialize cache for {CacheKey}", cacheKey);
					await _cache.RemoveAsync(cacheKey, cancellationToken);
				}
			}

			var result = await _inner.GetByIdAsync(id, cancellationToken);
			if (result is not null)
			{
				await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result, SerializerOptions), CacheOptions, cancellationToken);
			}
			return result;
		}

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<T>> GetPaginatedAsync(TFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var stamp = await GetStampAsync(cancellationToken);
			var cacheKey = BuildPaginatedCacheKey(stamp, filters, pagination);
			var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

			if (!string.IsNullOrEmpty(cached))
			{
				try
				{
					var result = JsonSerializer.Deserialize<PaginatedResult<T>>(cached, SerializerOptions);
					if (result is not null)
					{
						_logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
						return result;
					}
				}
				catch (JsonException ex)
				{
					_logger.LogWarning(ex, "Failed to deserialize cache for {CacheKey}", cacheKey);
					await _cache.RemoveAsync(cacheKey, cancellationToken);
				}
			}

			var fresh = await _inner.GetPaginatedAsync(filters, pagination, cancellationToken);
			await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(fresh, SerializerOptions), CacheOptions, cancellationToken);
			return fresh;
		}

		// ---------- CREATE ----------
		public async Task CreateAsync(T entity, CancellationToken cancellationToken)
		{
			await _inner.CreateAsync(entity, cancellationToken);
			await InvalidateCacheAsync(null, cancellationToken);
		}

		// ---------- UPDATE ----------
		public async Task UpdateAsync(Guid id, T entity, CancellationToken cancellationToken)
		{
			await _inner.UpdateAsync(id, entity, cancellationToken);
			await InvalidateCacheAsync(id, cancellationToken);
		}

		// ---------- DELETE ----------
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			await _inner.DeleteAsync(id, cancellationToken);
			await InvalidateCacheAsync(id, cancellationToken);
		}

		// ---------- HELPERS ----------
		private async Task<string> GetStampAsync(CancellationToken ct) =>
			await _stampManager.GetStampAsync(typeof(T).Name, ct);

		private Task InvalidateCacheAsync(Guid? id, CancellationToken ct)
		{
			var tasks = new List<Task>();
			if (id.HasValue)
				tasks.Add(_cache.RemoveAsync(BuildEntityCacheKey(id.Value), ct));

			tasks.Add(_stampManager.BumpStampAsync(typeof(T).Name, ct));
			return Task.WhenAll(tasks);
		}

		private string BuildEntityCacheKey(Guid id) =>
			$"{typeof(T).Name}:id:{id}";

		private string BuildAnyCacheKey(string stamp, TFilters filters) =>
			$"{typeof(T).Name}:v:{stamp}:any:{filters.CacheKey()}";

		private string BuildPaginatedCacheKey(string stamp, TFilters filters, PaginatedRequest pagination) =>
			$"{typeof(T).Name}:v:{stamp}:filter:{filters.CacheKey()}:page:{pagination.PageIndex}:{pagination.PageSize}";
	}
}
