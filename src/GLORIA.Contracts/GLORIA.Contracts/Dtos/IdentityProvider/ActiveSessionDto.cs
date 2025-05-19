namespace GLORIA.Contracts.Dtos.IdentityProvider
{
	public class ActiveSessionDto
	{
		public string Device { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; }

		public string? IpAddress { get; set; }

		public DateTime ExpiresAt { get; set; }

		public bool IsCurrent { get; set; } = false;
	}
}