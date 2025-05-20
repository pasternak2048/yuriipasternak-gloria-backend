using MongoDB.Driver;

namespace GLORIA.Contracts.Dtos.Common
{
	public abstract class BaseFilters
	{
		public abstract string CacheKey();

		public abstract FilterDefinition<T> ToFilter<T>() where T : class;
	}
}
