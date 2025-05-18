using BuildingBlocks.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Extensions
{
	public static class IdentityExtensions
	{
		public static void AddCurrentUser(this IServiceCollection services)
		{
			services.AddScoped<IUserIdentityProvider, UserIdentityProvider>();
		}
	}
}
