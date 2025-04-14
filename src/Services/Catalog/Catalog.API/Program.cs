using Catalog.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationServices(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

app.UseCustomMiddlewares();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
	await app.InitializeDatabaseAsync();
}

app.Run();
