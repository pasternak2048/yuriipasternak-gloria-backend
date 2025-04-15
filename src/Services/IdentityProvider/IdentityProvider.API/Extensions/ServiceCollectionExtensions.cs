namespace IdentityProvider.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddCorsPolicy();
			services.AddJwtAuthentication(configuration);
			services.AddIdentityCoreServices(configuration);
			services.AddDatabaseInfrastructure(configuration);
			services.AddExceptionHandlerServices();
			services.AddSwaggerDocumentation();
		}
	}
}
