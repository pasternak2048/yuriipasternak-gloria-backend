using Catalog.API.Services.Interfaces;
using Catalog.API.Services;

namespace Catalog.API.Extensions
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
