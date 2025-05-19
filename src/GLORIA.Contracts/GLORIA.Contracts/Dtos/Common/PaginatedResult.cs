namespace GLORIA.Contracts.Dtos.Common
{
	public class PaginatedResult<TEntity>(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
	where TEntity : class
	{
		public int PageIndex { get; } = pageIndex;

		public int PageSize { get; } = pageSize;

		public long Count { get; } = count;

		public IEnumerable<TEntity> Data { get; } = data;

		public static PaginatedResult<TEntity> Empty =>
			new(pageIndex: 0, pageSize: 0, count: 0, data: []);
	}
}
