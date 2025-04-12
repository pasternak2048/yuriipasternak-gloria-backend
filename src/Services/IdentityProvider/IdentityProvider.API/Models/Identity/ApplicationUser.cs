﻿using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.API.Models.Identity
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;
	}
}
