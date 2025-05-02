using BuildingBlocks.Filtering;
using BuildingBlocks.Infrastructure.Entities;
using BuildingBlocks.Pagination;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BuildingBlocks.Infrastructure
{
	public class CachedGenericRepository<T, TFilters> : IGenericRepository<T, TFilters>
		where T : class, IEntity
		where TFilters : BaseFilters
	{
		private readonly IGenericRepository<T, TFilters> _inner;
		private readonly IDistributedCache _cache;
		private readonly ILogger<CachedGenericRepository<T, TFilters>> _logger;

		private static readonly DistributedCacheEntryOptions CacheOptions = new()
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
		};

		public CachedGenericRepository(
			IGenericRepository<T, TFilters> inner,
			IDistributedCache cache,
			ILogger<CachedGenericRepository<T, TFilters>> logger)
		{
			_inner = inner;
			_cache = cache;
			_logger = logger;
		}

		// ---------- GET BY ID ----------
		public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var cacheKey = $"{typeof(T).Name}:id:{id}";
			var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

			if (!string.IsNullOrEmpty(cached))
			{
				_logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
				return JsonSerializer.Deserialize<T>(cached);
			}

			var entity = await _inner.GetByIdAsync(id, cancellationToken);
			if (entity is not null)
			{
				await _cache.SetStringAsync(
					cacheKey,
					JsonSerializer.Serialize(entity),
					CacheOptions,
					cancellationToken);
			}
			return entity;
		}

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<T>> GetPaginatedAsync(TFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var cacheKey = $"{typeof(T).Name}:filter:{filters.CacheKey()}:page:{pagination.PageIndex}:{pagination.PageSize}";
			var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

			if (!string.IsNullOrEmpty(cached))
			{
				_logger.LogDebug("Cache hit for {CacheKey}", cacheKey);
				return JsonSerializer.Deserialize<PaginatedResult<T>>(cached)!;
			}

			var result = await _inner.GetPaginatedAsync(filters, pagination, cancellationToken);
			await _cache.SetStringAsync(
				cacheKey,
				JsonSerializer.Serialize(result),
				CacheOptions,
				cancellationToken);
			return result;
		}

		// ---------- CREATE ----------
		public Task CreateAsync(T entity, CancellationToken cancellationToken) =>
			_inner.CreateAsync(entity, cancellationToken);

		// ---------- UPDATE ----------
		public Task UpdateAsync(Guid id, T entity, CancellationToken cancellationToken) =>
			_inner.UpdateAsync(id, entity, cancellationToken);

		// ---------- DELETE ----------
		public Task DeleteAsync(Guid id, CancellationToken cancellationToken) =>
			_inner.DeleteAsync(id, cancellationToken);
	}
}
