using BuildingBlocks.Security;
using BuildingBlocks.Security.Interfaces;
using BuildingBlocks.Security.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Extensions
{
	public static class SecurityExtensions
	{
		public static void AddSignatureValidation(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<SecurityValidationOptions>(configuration.GetSection("SecurityValidation"));
			services.AddSingleton<ISignatureValidator, SignatureValidator>();
		}
	}
}
