using YarpApiGatewayDesktop.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
	.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCorsPolicy();

var app = builder.Build();

app.UseRouting();

app.UseCors("AllowSwaggerFromGateway");

app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint("https://localhost:6062/swagger/v1/swagger.json", "IdentityProvider API");
	options.SwaggerEndpoint("https://localhost:6063/swagger/v1/swagger.json", "Catalog API");
	options.SwaggerEndpoint("https://localhost:6069/swagger/v1/swagger.json", "Photo API");
	options.DocumentTitle = "API Gateway Swagger";
	options.RoutePrefix = string.Empty;
});

app.MapReverseProxy();

app.Run();
