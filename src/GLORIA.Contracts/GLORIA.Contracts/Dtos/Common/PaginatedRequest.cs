namespace GLORIA.Contracts.Dtos.Common
{
	public class PaginatedRequest
	{
		public int PageIndex { get; set; } = 1;

		public int PageSize { get; set; } = 10;

		public int Skip => (PageIndex - 1) * PageSize;
	}
}
