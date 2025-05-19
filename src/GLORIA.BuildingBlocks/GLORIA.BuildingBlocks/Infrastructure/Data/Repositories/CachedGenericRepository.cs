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

		public async Task<PaginatedResult<T>> GetPaginatedAsync(TFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var stamp = await _stampManager.GetStampAsync(typeof(T).Name, cancellationToken);

			var cacheKey = $"{typeof(T).Name}:v:{stamp}:filter:{filters.CacheKey()}:page:{pagination.PageIndex}:{pagination.PageSize}";
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

		public async Task CreateAsync(T entity, CancellationToken cancellationToken)
		{
			await _inner.CreateAsync(entity, cancellationToken);
			await _stampManager.BumpStampAsync(typeof(T).Name, cancellationToken);
		}

		public async Task UpdateAsync(Guid id, T entity, CancellationToken cancellationToken)
		{
			await _inner.UpdateAsync(id, entity, cancellationToken);
			await _cache.RemoveAsync($"{typeof(T).Name}:id:{id}", cancellationToken);
			await _stampManager.BumpStampAsync(typeof(T).Name, cancellationToken);
		}

		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			await _inner.DeleteAsync(id, cancellationToken);
			await _cache.RemoveAsync($"{typeof(T).Name}:id:{id}", cancellationToken);
			await _stampManager.BumpStampAsync(typeof(T).Name, cancellationToken);
		}
	}
}
