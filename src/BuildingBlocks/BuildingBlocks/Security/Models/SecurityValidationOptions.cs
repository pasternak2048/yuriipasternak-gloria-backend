using BuildingBlocks.Configurations;

namespace BuildingBlocks.Security.Models
{
	public class SecurityValidationOptions
	{
		public ServiceConfig Service { get; set; } = default!;

		public int ValidationWindowMinutes { get; set; } = 60;
	}
}
