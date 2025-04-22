using BuildingBlocks.Exceptions.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Extensions
{
	public static class ExceptionHandlerExtensions
	{
		public static void AddExceptionHandlerServices(this IServiceCollection services)
		{
			services.AddExceptionHandler<CustomExceptionHandler>();
		}
	}
}
