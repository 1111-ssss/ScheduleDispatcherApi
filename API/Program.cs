using API.Endpoints;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Swagger
builder.Services.AddCustomSwagger();

//MediatR + FluentValidationBehavior + FluentValidation + Services
builder.Services.AddCustomServices();

//Authentication + Authorization
builder.Services.AddAuthServices(builder.Configuration);

//DbContext
builder.Services.AddCustomDbContext(builder.Configuration);

var app = builder.Build();

app.UseCustomMiddleware();

app.MapCustomRoutes();

app.Run();