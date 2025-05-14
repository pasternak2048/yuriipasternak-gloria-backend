using IdentityProvider.API.Models.Entities;

namespace IdentityProvider.API.Services.Interfaces
{
	public interface IRefreshTokenService
	{
		Task SaveAsync(RefreshToken token);

		Task<RefreshToken?> GetByTokenAsync(string token);

		Task RevokeAsync(RefreshToken token, string? ip = null);

		Task<IEnumerable<RefreshToken>> GetActiveTokensByUser(Guid userId);
	}
}
