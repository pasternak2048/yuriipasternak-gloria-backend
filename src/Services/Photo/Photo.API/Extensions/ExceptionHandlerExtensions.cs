using BuildingBlocks.Exceptions.Handler;

namespace Photo.API.Extensions
{
	public static class ExceptionHandlerExtensions
	{
		public static void AddExceptionHandlerServices(this IServiceCollection services)
		{
			services.AddExceptionHandler<CustomExceptionHandler>();
		}

		public static WebApplication UseCustomExceptionHandler(this WebApplication app)
		{
			app.UseExceptionHandler(options => { /* logging etc */ });
			return app;
		}
	}
}
