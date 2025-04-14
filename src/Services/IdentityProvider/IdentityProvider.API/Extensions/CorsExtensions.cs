namespace IdentityProvider.API.Extensions
{
	public static class CorsExtensions
	{
		public static void AddCorsPolicy(this IServiceCollection services)
		{
			services.AddCors(opt =>
			{
				opt.AddDefaultPolicy(builder =>
					builder.AllowAnyOrigin()
						   .AllowAnyMethod()
						   .AllowAnyHeader());
			});
		}
	}
}
