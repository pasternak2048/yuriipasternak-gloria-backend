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

		public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<ITokenService, TokenService>();

			services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
			{
				ConfigureIdentityOptions(options);
			})
			.AddEntityFrameworkStores<IdentityProviderDbContext>()
			.AddDefaultTokenProviders();
		}

		private static void ConfigureIdentityOptions(IdentityOptions options)
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
		}

		public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("Database");

			services.AddDbContext<IdentityProviderDbContext>(options =>
				options.UseSqlServer(connectionString));
		}

		public static void AddExceptionHandlerServices(this IServiceCollection services)
		{
			services.AddExceptionHandler<CustomExceptionHandler>();
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

		public static WebApplication UseExceptionHandlerServices(this WebApplication app)
		{
			app.UseExceptionHandler(options =>
			{

			});

			return app;
		}

		public static async Task InitialiseDatabaseAsync(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();

			var context = scope.ServiceProvider.GetRequiredService<IdentityProviderDbContext>();

			context.Database.MigrateAsync().GetAwaiter().GetResult();

			var dataSeeder = new IdentityDataSeeder();

			await IdentityDataSeeder.SeedAsync(app.Services);
		}
	}
}
