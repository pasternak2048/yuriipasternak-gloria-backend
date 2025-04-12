using IdentityProvider.API.Configurations;
using IdentityProvider.API.Models.Identity;
using IdentityProvider.API.Services.Interfaces;
using IdentityProvider.API.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore.Diagnostics;
using IdentityProvider.API.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
			services.AddScoped<ITokenService, TokenService>();

			services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
				options.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<IdentityDbContext>()
			.AddDefaultTokenProviders();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

				options.RequireHttpsMetadata = false; // for local
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
		}

		public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Database");

			services.AddDbContext<IdentityDbContext>(options =>
				options.UseSqlServer(connectionString));
		}
	}
}
