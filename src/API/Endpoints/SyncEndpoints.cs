using Microsoft.AspNetCore.Mvc;
using MediatR;
using Domain.Abstractions.Result;
using Application.Features.Sync.SyncData;

namespace API.Endpoints;

public static class SyncEndpoints
{
    public static IEndpointRouteBuilder MapSyncEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sync")
            .WithTags("Синхронизация с Единым Колледжем");

        group.MapPost("/", SyncDataAsync)
            .WithName("Sync")
            .WithSummary("Запрос на синхронизацию с Единым Колледжем")
            .WithDescription("Позволяет администратору запросить синхронизацию базы данных с Единым Колледжем.")
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        return group;
    }
    private static async Task<IResult> SyncDataAsync(
        [FromServices] IMediator _mediator
    )
    {
        var result = await _mediator.Send(new SyncDataCommand());

        return result.ToApiResult();
    }
}