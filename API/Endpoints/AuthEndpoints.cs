using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Auth.Login;

namespace API.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Аутентификация");
            // .MapSwagger();

        group.MapPost("/login", LoginAsync);

        return group;
    }
    private static async Task<IActionResult> LoginAsync(
        [FromServices] IMediator _mediator,
        [FromBody] LoginUserCommand command
    )
    {
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
}