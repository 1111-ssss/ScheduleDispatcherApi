using Microsoft.AspNetCore.Mvc;
using MediatR;
using Domain.Abstractions.Result;
using Application.Features.Dispatcher.GetWorkload;
using Application.Features.Dispatcher.Common;
using Application.Features.Dispatcher.SaveWorkload;
using Application.Features.Dispatcher.FinalizeDaySchedule;
using Application.Features.Dispatcher.GetAllLessons;

namespace API.Endpoints;

public static class DispatcherEndpoints
{
    public static IEndpointRouteBuilder MapDispatcherEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dispatcher")
            .WithTags("Функции диспетчера");

        group.MapGet("/get", GetWorkloadAsync)
            .WithName("Get Workload")
            .WithSummary("Запрос на получение расчасовки")
            .WithDescription("Позволяет получить расчасовку и снятие для предмета по группе, семестру и преподавателю. Возвращает расчасовку и снятие.")
            .Accepts<GetWorkloadQuery>("application/json")
            .Produces<WorkloadSummaryDTO>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapPost("/save", SaveWorkloadAsync)
            .WithName("Save Workload")
            .WithSummary("Запрос на сохранение расчасовки")
            .WithDescription("Позволяет сохранить расчасовку и снятие для предмета по группе, семестру и преподавателю.")
            .Accepts<SaveWorkloadCommand>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapPost("/finalize", FinalizeDayScheduleAsync)
            .WithName("Finalize Workload")
            .WithSummary("Запрос на сохранение расписания дня")
            .WithDescription("Позволяет сохранять расписание дня для группы на определенный день.")
            .Accepts<FinalizeDayScheduleCommand>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapGet("/lessons", GetAllLessonsAsync)
            .WithName("Get Lessons Info")
            .WithSummary("Запрос на получение информации о предметах")
            .WithDescription("Позволяет получить информацию о предметах, в каком семестре они проводятся, на каком курсу и списку групп.")
            .Produces<AllLessonsDTO>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        return group;
    }
    private static async Task<IResult> GetWorkloadAsync(
        [FromServices] IMediator _mediator,
        [AsParameters] GetWorkloadQuery query
    )
    {
        var result = await _mediator.Send(query);

        return result.ToApiResult();
    }
    private static async Task<IResult> SaveWorkloadAsync(
        [FromServices] IMediator _mediator,
        [FromBody] SaveWorkloadCommand command
    )
    {
        var result = await _mediator.Send(command);

        return result.ToApiResult();
    }
    private static async Task<IResult> FinalizeDayScheduleAsync(
        [FromServices] IMediator _mediator,
        [FromBody] FinalizeDayScheduleCommand command
    )
    {
        var result = await _mediator.Send(command);

        return result.ToApiResult();
    }
    private static async Task<IResult> GetAllLessonsAsync(
        [FromServices] IMediator _mediator
    )
    {
        var result = await _mediator.Send(new GetAllLessonsQuery());

        return result.ToApiResult();
    }
}