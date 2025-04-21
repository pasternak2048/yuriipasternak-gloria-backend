using Photo.API.Services;
using Photo.API.Services.Interfaces;

namespace Photo.API.Extensions
{
	public static class HttpContextExtensions
	{
		public static void AddHttpContextServices(this IServiceCollection services)
		{
			services.AddHttpContextAccessor();
			services.AddScoped<IUserContextService, UserContextService>();
		}
	}
}
