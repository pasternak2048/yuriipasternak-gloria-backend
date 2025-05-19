using GLORIA.BuildingBlocks.Configuration;

namespace GLORIA.BuildingBlocks.Security
{
	public class SecurityValidationOptions
	{
		public ServiceConfig Service { get; set; } = default!;

		public int ValidationWindowMinutes { get; set; } = 60;
	}
}
