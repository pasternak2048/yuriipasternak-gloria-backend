namespace GLORIA.BuildingBlocks.Exceptions
{
	public class ForbiddenAccessException : Exception
	{
		public ForbiddenAccessException(string message) : base(message)
		{
		}

		public ForbiddenAccessException(string message, string details) : base(message)
		{
			Details = details;
		}

		public string? Details { get; }
	}
}
