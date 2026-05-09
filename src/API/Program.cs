using API.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

//MediatR + FluentValidationBehavior + FluentValidation + Services
builder.Services.AddCustomServices();

//Authentication + Authorization
builder.Services.AddAuthServices(builder.Configuration);

//DbContext
builder.Services.AddCustomDbContext(builder.Configuration);

builder.Services.AddMessaging(builder.Configuration);

var app = builder.Build();

app.UseCustomMiddleware();
app.UseRateLimiter();

//Endpoints
app.MapCustomRoutes();

app.Run();
