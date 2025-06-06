using GLORIA.YarpApiGatewayDesktop.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseCustomMiddlewares();

app.Run();
