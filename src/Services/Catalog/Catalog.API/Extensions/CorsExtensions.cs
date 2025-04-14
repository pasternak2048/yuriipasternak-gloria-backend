namespace Catalog.API.Extensions
{
	public static class CorsExtensions
	{
		public static void AddCorsPolicy(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
					builder.AllowAnyOrigin()
						   .AllowAnyMethod()
						   .AllowAnyHeader());
			});
		}
	}
}
