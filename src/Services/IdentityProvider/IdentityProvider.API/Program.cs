using IdentityProvider.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddDatabaseServices(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();