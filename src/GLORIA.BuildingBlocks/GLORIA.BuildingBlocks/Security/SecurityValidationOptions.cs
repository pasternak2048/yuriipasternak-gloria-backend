using GLORIA.BuildingBlocks.Configuration;

namespace GLORIA.BuildingBlocks.Security
{
	public class SecurityValidationOptions
	{
		public bool Enabled { get; set; }

        public ServiceConfig Service { get; set; } = default!;

        public List<TrustedService> TrustedServices { get; set; } = new();

        public int ValidationWindowMinutes { get; set; } = 60;
	}

    public class TrustedService
    {
        public string Name { get; set; } = default!;

        public string Secret { get; set; } = default!;
    }
}
