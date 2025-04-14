using BuildingBlocks.Exceptions.Handler;
using IdentityProvider.API.Configurations;
using IdentityProvider.API.Data;
using IdentityProvider.API.Data.Seed;
using IdentityProvider.API.Models.Identity;
using IdentityProvider.API.Services;
using IdentityProvider.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
		}
	}
}
