using Microsoft.AspNetCore.Identity;

namespace GLORIA.IdentityProvider.API.Models.Entities.Identity
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;
	}
}
