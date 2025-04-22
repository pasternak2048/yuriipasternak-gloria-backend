using Microsoft.Extensions.Caching.Distributed;
using Photo.API.Models;
using Photo.API.Repositories.Interfaces;
using System.Text.Json;

namespace Photo.API.Repositories
{
	public class CachedRealtyPhotoRepository : IRealtyPhotoRepository
	{
		private readonly IRealtyPhotoRepository _inner;
		private readonly IDistributedCache _cache;
		private readonly ILogger<CachedRealtyPhotoRepository> _logger;

		private static readonly DistributedCacheEntryOptions _cacheOptions =
			new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
			};

		public CachedRealtyPhotoRepository(
			IRealtyPhotoRepository inner,
			IDistributedCache cache,
			ILogger<CachedRealtyPhotoRepository> logger)
		{
			_inner = inner;
			_cache = cache;
			_logger = logger;
		}

		// ---------------- GET BY ID ----------------
		public async Task<RealtyPhotoMetadata?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var cacheKey = $"realtyphoto:{id}";
			var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

			if (!string.IsNullOrEmpty(cached))
			{
				_logger.LogInformation("Retrieved realty photo {Id} from cache", id);
				return JsonSerializer.Deserialize<RealtyPhotoMetadata>(cached);
			}

			var metadata = await _inner.GetByIdAsync(id, cancellationToken);

			if (metadata != null)
			{
				await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(metadata), _cacheOptions, cancellationToken);
			}

			return metadata;
		}

		// ---------------- GET BY REALTY ID ----------------
		public async Task<IEnumerable<RealtyPhotoMetadata>> GetByRealtyIdAsync(Guid realtyId, CancellationToken cancellationToken)
		{
			var cacheKey = $"realtyphoto:realty:{realtyId}";
			var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);

			if (!string.IsNullOrEmpty(cached))
			{
				_logger.LogInformation("Retrieved realty photos for RealtyId {RealtyId} from cache", realtyId);
				return JsonSerializer.Deserialize<List<RealtyPhotoMetadata>>(cached) ?? [];
			}

			var list = (await _inner.GetByRealtyIdAsync(realtyId, cancellationToken)).ToList();

			await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(list), _cacheOptions, cancellationToken);

			return list;
		}

		// ---------------- ADD ----------------
		public async Task AddAsync(RealtyPhotoMetadata metadata, CancellationToken cancellationToken)
		{
			await _inner.AddAsync(metadata, cancellationToken);

			await _cache.RemoveAsync($"realtyphoto:realty:{metadata.RealtyId}", cancellationToken);
		}

		// ---------------- DELETE BY ID ----------------
		public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			var existing = await _inner.GetByIdAsync(id, cancellationToken);

			if (existing != null)
			{
				await _inner.DeleteByIdAsync(id, cancellationToken);

				await _cache.RemoveAsync($"realtyphoto:{id}", cancellationToken);
				await _cache.RemoveAsync($"realtyphoto:realty:{existing.RealtyId}", cancellationToken);
			}
			else
			{
				_logger.LogWarning("Tried to delete realty photo {Id}, but it was not found", id);
			}
		}

		// ---------------- DELETE BY REALTY ID ----------------
		public async Task DeleteByRealtyIdAsync(Guid realtyId, CancellationToken cancellationToken)
		{
			await _inner.DeleteByRealtyIdAsync(realtyId, cancellationToken);

			await _cache.RemoveAsync($"realtyphoto:realty:{realtyId}", cancellationToken);
		}
	}
}
