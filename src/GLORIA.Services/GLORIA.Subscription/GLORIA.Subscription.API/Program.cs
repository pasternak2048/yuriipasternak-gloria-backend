using GLORIA.Subscription.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseCustomMiddlewares();

app.Run();