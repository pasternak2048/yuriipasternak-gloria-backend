using GLORIA.BuildingBlocks.Extensions.Application;
using GLORIA.BuildingBlocks.Extensions.Infrastructure;

namespace GLORIA.IdentityProvider.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddJwtAuthentication(configuration);
			services.AddCorsPolicy();
			services.AddExceptionHandlerServices();
			services.AddSwaggerDocumentation("Identity Provider API");
			services.AddIdentityCoreServices(configuration);
			services.AddDatabaseInfrastructure(configuration);
			services.AddSignatureValidation(configuration);
			services.AddControllers();
		}
	}
}
