using Catalog.API.Services.Interfaces;
using Catalog.API.Services;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Repositories;
using System.Reflection;

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
			services.AddSwaggerDocumentation();

			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddScoped<IRealtyService, RealtyService>();
			services.AddScoped<IRealtyRepository, RealtyRepository>();
		}
	}
}
