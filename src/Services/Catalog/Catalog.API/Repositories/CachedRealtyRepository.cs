using BuildingBlocks.Infrastructure;
using BuildingBlocks.Pagination;
using Catalog.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Catalog.API.Repositories
{
	public class CachedRealtyRepository : IGenericRepository<Realty, RealtyFilters>
	{
		private readonly IGenericRepository<Realty, RealtyFilters> _inner;
		private readonly IDistributedCache _cache;
		private readonly ILogger<CachedRealtyRepository> _logger;

		private static readonly DistributedCacheEntryOptions _cacheOptions =
			new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
			};

		public CachedRealtyRepository(
			IGenericRepository<Realty, RealtyFilters> inner,
			IDistributedCache cache,
			ILogger<CachedRealtyRepository> logger)
		{
			_inner = inner;
			_cache = cache;
			_logger = logger;
		}

		// ---------- GET BY ID ----------
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

		// ---------- GET PAGINATED ----------
		public async Task<PaginatedResult<Realty>> GetPaginatedAsync(RealtyFilters filters, PaginatedRequest pagination, CancellationToken cancellationToken)
		{
			var cacheKey = $"realty:paginated:{filters.CacheKey()}:page={pagination.PageIndex}:size={pagination.PageSize}";
			var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

			if (!string.IsNullOrEmpty(cached))
			{
				_logger.LogInformation("Retrieved paginated realties from cache");
				return JsonSerializer.Deserialize<PaginatedResult<Realty>>(cached) ?? PaginatedResult<Realty>.Empty;
			}

			var result = await _inner.GetPaginatedAsync(filters, pagination, cancellationToken);
			await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result), _cacheOptions, cancellationToken);

			return result;
		}

		// ---------- CREATE ----------
		public async Task CreateAsync(Realty entity, CancellationToken cancellationToken)
		{
			await _inner.CreateAsync(entity, cancellationToken);
			await InvalidateCache(entity.Id, cancellationToken);
		}

		// ---------- UPDATE ----------
		public async Task UpdateAsync(Guid id, Realty updated, CancellationToken cancellationToken)
		{
			await _inner.UpdateAsync(id, updated, cancellationToken);
			await InvalidateCache(id, cancellationToken);
		}

		// ---------- DELETE ----------
		public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
		{
			await _inner.DeleteAsync(id, cancellationToken);
			await InvalidateCache(id, cancellationToken);
		}

		// ---------- HELPER ----------
		private async Task InvalidateCache(Guid id, CancellationToken cancellationToken)
		{
			await _cache.RemoveAsync($"realty:{id}", cancellationToken);
			_logger.LogInformation("Invalidated cache for realty {Id} and related paginated data", id);
		}
	}
}
