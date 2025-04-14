using Catalog.API.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtServices(builder.Configuration);

builder.Services.AddCorsPolicy();

builder.Services.AddMongoServices(builder.Configuration);

builder.Services.AddExceptionHandlerServices();

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
