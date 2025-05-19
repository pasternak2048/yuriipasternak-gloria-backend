using GLORIA.IdentityProvider.API.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace GLORIA.IdentityProvider.API.Services.Interfaces
{
	public interface IJwtTokenService
	{
		public Task<string> GenerateAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);
	}
}
