using IdentityProvider.API.Models.Identity;

namespace IdentityProvider.API.Services.Interfaces
{
	public interface ITokenService
	{
		public Task<string> GenerateTokenAsync(ApplicationUser user);
	}
}
