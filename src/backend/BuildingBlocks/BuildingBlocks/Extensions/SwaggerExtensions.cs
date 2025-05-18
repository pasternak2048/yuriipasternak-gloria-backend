using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BuildingBlocks.Extensions
{
	public static class SwaggerExtensions
	{
		public static void AddSwaggerDocumentation(this IServiceCollection services, string title)
		{
			services.AddEndpointsApiExplorer();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = "v1" });

				var jwtSecurityScheme = new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Description = "Enter JWT Bearer token",
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

				c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

				c.AddSecurityDefinition("X-Service-Signature", new OpenApiSecurityScheme
				{
					Description = "Service signature (SHA512) for request validation",
					Name = "X-Service-Signature",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "X-Service-Signature"
				});

				c.AddSecurityDefinition("X-Service-Name", new OpenApiSecurityScheme
				{
					Description = "Name of the calling service",
					Name = "X-Service-Name",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "X-Service-Name"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{ jwtSecurityScheme, Array.Empty<string>() },
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "X-Service-Signature"
							}
						},
						Array.Empty<string>()
					},
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "X-Service-Name"
							}
						},
						Array.Empty<string>()
					}
				});
			});
		}
	}
}
