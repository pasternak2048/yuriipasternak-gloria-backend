using IdentityProvider.API.Data;
using IdentityProvider.API.Models.Entities;
using IdentityProvider.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.API.Services
{
	public class RefreshTokenService : IRefreshTokenService
	{
		private readonly IdentityProviderDbContext _context;

		public RefreshTokenService(IdentityProviderDbContext context)
		{
			_context = context;
		}

		public async Task SaveAsync(RefreshToken token)
		{
			if (await _context.RefreshTokens.AnyAsync(x => x.Token == token.Token))
				throw new InvalidOperationException("Duplicate refresh token.");

			_context.RefreshTokens.Add(token);
			await _context.SaveChangesAsync();
		}

		public Task<RefreshToken?> GetByTokenAsync(string token)
		{
			return _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
		}

		public async Task RevokeAsync(RefreshToken token, string? ip = null)
		{
			token.IsRevoked = true;
			token.RevokedAt = DateTime.UtcNow;
			token.RevokedByIp = ip;
			await _context.SaveChangesAsync();
		}

		public Task<IEnumerable<RefreshToken>> GetActiveTokensByUser(Guid userId)
		{
			return Task.FromResult<IEnumerable<RefreshToken>>(
				_context.RefreshTokens
					.Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
					.ToList()
			);
		}
	}
}
