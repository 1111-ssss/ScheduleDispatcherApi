using Application;
using Application.Abstractions.Interfaces.Auth;
using Application.Common.Behaviors;
using FluentValidation;
using Infrastructure.Abstractions.Interfaces.Auth;
using Infrastructure.Services.Auth;
using MediatR;

namespace API.Extensions;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        // HttpContextAccessor
        services.AddHttpContextAccessor();

        // MediatR + FluentValidationBehavior
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();

        // Auth
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}