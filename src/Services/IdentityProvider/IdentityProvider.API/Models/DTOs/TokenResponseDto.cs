﻿namespace IdentityProvider.API.Models.DTOs
{
	public class TokenResponseDto
	{
		public Guid UserId { get; set; }

		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string UserName { get; set; } = string.Empty;

		public List<string> Roles { get; set; } = new();

		public string Token { get; set; } = string.Empty;
	}
}
