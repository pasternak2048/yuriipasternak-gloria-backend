using Catalog.API.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Catalog.API.Extensions
{
	public static class AuthenticationExtensions
	{
		public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
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
	}
}
