using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Auth.Login;
using Domain.Model.Result;
using Application.Features.Auth.Common;
using Application.Features.Auth.Refresh;

namespace API.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Аутентификация");

        group.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("Вход в систему")
            .WithDescription("Позволяет пользователю войти в систему, предоставив имя пользователя и пароль. В случае успешной аутентификации возвращает JWT-токен для доступа к защищенным ресурсам API.")
            .Accepts<LoginUserQuery>("application/json")
            .Produces<AuthDTO>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapPost("/refresh", RefreshAsync)
            .WithName("Refresh")
            .WithSummary("Обновление токена")
            .WithDescription("Позволяет обновить JWT-токен, используя refresh-токен. В случае успешного обновления возвращает новый JWT-токен.")
            .Accepts<RefreshTokenQuery>("application/json")
            .Produces<AuthDTO>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        return group;
    }
    private static async Task<IResult> LoginAsync(
        [FromServices] IMediator _mediator,
        [FromBody] LoginUserQuery command
    )
    {
        var result = await _mediator.Send(command);

        return result.ToApiResult();
    }

    private static async Task<IResult> RefreshAsync(
        [FromServices] IMediator _mediator
    )
    {
        var result = await _mediator.Send(new RefreshTokenQuery());

        return result.ToApiResult();
    }
}