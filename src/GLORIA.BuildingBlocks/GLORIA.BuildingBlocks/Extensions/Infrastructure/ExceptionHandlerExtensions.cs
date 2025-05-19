using GLORIA.BuildingBlocks.Middleware;
using Microsoft.Extensions.DependencyInjection;

namespace GLORIA.BuildingBlocks.Extensions.Infrastructure
{
	public static class ExceptionHandlerExtensions
	{
		public static void AddExceptionHandlerServices(this IServiceCollection services)
		{
			services.AddExceptionHandler<ExceptionHandlerMiddleware>();
		}
	}
}
