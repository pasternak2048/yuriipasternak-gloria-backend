using GLORIA.BuildingBlocks.Extensions.Application;
using GLORIA.BuildingBlocks.Extensions.Infrastructure;
using GLORIA.DocumentStorage.API.Services;
using GLORIA.DocumentStorage.API.Services.Interfaces;

namespace GLORIA.DocumentStorage.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddJwtAuthentication(configuration);
			services.AddCorsPolicy();
			services.AddExceptionHandlerServices();
			services.AddSwaggerDocumentation("Document Storage API");
			services.AddControllers();
			services.AddScoped<IDocumentStorageService, DocumentStorageService>();
			services.AddScoped<IFileValidatorService, FileValidatorService>();
		}
	}
}