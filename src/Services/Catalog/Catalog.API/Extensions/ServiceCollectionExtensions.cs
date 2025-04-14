namespace Catalog.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCorsPolicy();
			services.AddJwtAuthentication(configuration);
			services.AddMongoInfrastructure(configuration);
			services.AddHttpContextServices();
			services.AddExceptionHandlerServices();
		}
	}
}
