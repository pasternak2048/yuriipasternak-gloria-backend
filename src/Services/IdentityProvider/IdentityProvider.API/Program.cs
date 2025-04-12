using IdentityProvider.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddDatabaseServices(builder.Configuration);

builder.Services.AddExceptionHandlerServices();

builder.Services.AddCorsPolicy();

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandlerServices();

if (app.Environment.IsDevelopment())
{
	await app.InitialiseDatabaseAsync();
}

app.Run();