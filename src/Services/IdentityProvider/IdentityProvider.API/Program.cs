using BuildingBlocks.Extensions;
using IdentityProvider.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationServices(builder.Configuration);

var app = builder.Build();

await app.UseCustomMiddlewares();

app.Run();