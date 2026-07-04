using API.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomServices();

builder.Services.AddAuthServices(builder.Configuration);

builder.Services.AddCustomDbContext(builder.Configuration);

builder.Services.AddMessaging(builder.Configuration);

var app = builder.Build();

await app.ApplyMigrationsAsync();

app.UseCustomMiddleware();
app.UseRateLimiter();

app.MapCustomRoutes();

app.Run();
