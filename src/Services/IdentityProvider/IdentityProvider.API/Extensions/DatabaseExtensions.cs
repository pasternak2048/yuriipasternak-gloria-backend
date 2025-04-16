using IdentityProvider.API.Data.Seed;
using IdentityProvider.API.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.API.Extensions
{
	public static class DatabaseExtensions
	{
		public static void AddDatabaseInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Database");

			services.AddDbContext<IdentityProviderDbContext>(options =>
				options.UseSqlServer(connectionString));
		}
	}
}
