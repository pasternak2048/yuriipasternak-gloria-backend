using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Catalog.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddJwtServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidIssuer = jwtSettings.Issuer,
						ValidAudience = jwtSettings.Audience,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
						ValidateIssuerSigningKey = true
					};
				});

			services.AddAuthorization();
		}

		public static void AddCorsPolicy(this IServiceCollection services)
		{
			services.AddCors(opt =>
			{
				opt.AddDefaultPolicy(builder => builder
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());
			});
		}

		public static void AddExceptionHandlerServices(this IServiceCollection services)
		{
			services.AddExceptionHandler<CustomExceptionHandler>();
		}

		public static WebApplication UseExceptionHandlerServices(this WebApplication app)
		{
			app.UseExceptionHandler(options =>
			{

			});

			return app;
		}
	}
}
