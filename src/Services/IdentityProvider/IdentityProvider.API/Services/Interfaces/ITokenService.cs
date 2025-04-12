using IdentityProvider.API.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.API.Services.Interfaces
{
	public interface ITokenService
	{
		public Task<string> GenerateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);
	}
}
