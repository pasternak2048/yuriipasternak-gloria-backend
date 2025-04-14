using Catalog.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtServices(builder.Configuration);

builder.Services.AddCorsPolicy();

builder.Services.AddExceptionHandlerServices();

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandlerServices();

app.Run();
