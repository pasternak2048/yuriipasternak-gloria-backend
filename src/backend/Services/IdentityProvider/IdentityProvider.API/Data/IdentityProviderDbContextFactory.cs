using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.API.Data
{
	public class IdentityProviderDbContextFactory : IDesignTimeDbContextFactory<IdentityProviderDbContext>
	{
		public IdentityProviderDbContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.Development.json", optional: true)
				.AddJsonFile("appsettings.json", optional: true)
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<IdentityProviderDbContext>();

			var connectionString = configuration.GetConnectionString("Database");

			optionsBuilder.UseSqlServer(connectionString);

			return new IdentityProviderDbContext(optionsBuilder.Options);
		}
	}
}
