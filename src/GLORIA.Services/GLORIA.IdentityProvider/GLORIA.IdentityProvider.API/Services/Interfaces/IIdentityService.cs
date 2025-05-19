using GLORIA.Contracts.Dtos.IdentityProvider;

namespace GLORIA.IdentityProvider.API.Services.Interfaces
{
	public interface IIdentityService
	{
		Task<TokenResponseDto> LoginAsync(LoginDto dto, string? ip, string? device);

		Task<TokenResponseDto> RegisterAsync(RegisterDto dto, string? ip, string? device);

		Task<TokenResponseDto> RefreshAsync(RefreshRequestDto dto, string? ip, string? device);

		Task<IEnumerable<ActiveSessionDto>> GetActiveSessionsAsync(string userId, string? currentRefreshToken);

		Task RevokeSessionAsync(string userId, string refreshToken, string? ip);

		Task LogoutAsync(string userId, string refreshToken, string? ip);

		Task LogoutFromAllAsync(string userId, string? ip);
	}
}
