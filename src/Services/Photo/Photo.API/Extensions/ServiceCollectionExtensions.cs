using Photo.API.Services.Interfaces;
using Photo.API.Services;
using System.Reflection;
using Photo.API.Repositories;
using Photo.API.Repositories.Interfaces;

namespace Photo.API.Extensions
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
			services.AddScoped<IRealtyPhotoRepository, RealtyPhotoRepository>();
			services.AddScoped<IRealtyPhotoService, RealtyPhotoService>();
			services.AddScoped<IFileStorageService, FileStorageService>();
			services.AddSingleton<IFileValidatorService, FileValidatorService>();
		}
	}
}
