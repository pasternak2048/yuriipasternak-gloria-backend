using BuildingBlocks.Extensions.Application;
using BuildingBlocks.Extensions.Infrastructure;
using DocumentStorage.API.Services;
using DocumentStorage.API.Services.Interfaces;

namespace DocumentStorage.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddJwtAuthentication(configuration);
			services.AddCorsPolicy();
			services.AddExceptionHandlerServices();
			services.AddSwaggerDocumentation("Document Storage API");
			services.AddSignatureValidation(configuration);
			services.AddControllers();
			services.AddScoped<IDocumentStorageService, DocumentStorageService>();
			services.AddScoped<IFileValidatorService, FileValidatorService>();
		}
	}
}