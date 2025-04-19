using Photo.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationServices(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(options => { });

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Photo API v1");
		options.RoutePrefix = string.Empty;
	});
}

app.Run();
