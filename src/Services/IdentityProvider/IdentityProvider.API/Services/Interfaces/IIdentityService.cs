using IdentityProvider.API.Models.DTOs;

namespace IdentityProvider.API.Services.Interfaces
{
	public interface IIdentityService
	{
		Task<TokenResponseDto> LoginAsync(LoginDto dto);

		Task<TokenResponseDto> RegisterAsync(RegisterDto dto);
	}
}
