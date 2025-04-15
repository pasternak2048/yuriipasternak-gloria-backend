using Microsoft.OpenApi.Models;

namespace IdentityProvider.API.Extensions
{
	public static class SwaggerServiceExtensions
	{
		public static void AddSwaggerDocumentation(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Identity Provider API",
					Version = "v1"
				});

				// JWT Auth support
				var securityScheme = new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Description = "Enter JWT Bearer token **_only_**",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				};

				options.AddSecurityDefinition("Bearer", securityScheme);
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						securityScheme, new string[] {}
					}
				});
			});
		}
	}
}
