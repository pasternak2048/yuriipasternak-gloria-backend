using GLORIA.BuildingBlocks.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GLORIA.BuildingBlocks.Extensions.Application
{
	public static class SecurityExtensions
	{
		public static void AddSignatureValidation(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<SecurityValidationOptions>(configuration.GetSection("SecurityValidation"));
			services.AddSingleton<ISignatureValidator, SignatureValidator>();
			services.AddSingleton<ISignatureService, SignatureService>();
			services.AddSingleton<ISignedHttpClient, SignedHttpClient>();
		}
	}
}
