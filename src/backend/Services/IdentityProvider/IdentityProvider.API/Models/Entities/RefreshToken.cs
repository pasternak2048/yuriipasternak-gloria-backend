namespace IdentityProvider.API.Models.Entities
{
	public class RefreshToken
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Token { get; set; } = string.Empty;
		public Guid UserId { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime ExpiresAt { get; set; }

		public string? CreatedByIp { get; set; }
		public string? Device { get; set; }
		public string? Location { get; set; }

		public bool IsRevoked { get; set; } = false;
		public DateTime? RevokedAt { get; set; }
		public string? RevokedByIp { get; set; }
		public string? ReplacedByToken { get; set; }
	}
}
