using Microsoft.AspNetCore.Mvc;
using MediatR;
using Domain.Model.Result;
using Application.Features.Dispatcher.GetWorkload;
using Application.Features.Dispatcher.Common;
using Application.Features.Dispatcher.SaveWorkload;
using Application.Features.Dispatcher.Commands.SaveDraftSchedule;
using Application.Features.Dispatcher.FinalizeDaySchedule;
using Application.Features.Dispatcher.GetAllLessons;
using Application.Features.Dispatcher.GetCabinets;
using Application.Features.Dispatcher.GetGroups;
using Application.Features.Dispatcher.GetTeachers;

namespace API.Endpoints;

public static class DispatcherEndpoints
    {
    public static IEndpointRouteBuilder MapDispatcherEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dispatcher")
            .RequireRateLimiting("DefaultLimiter")
            .RequireAuthorization()
            .WithTags("Функции диспетчера");

        group.MapGet("/get", GetWorkloadAsync)
            .WithSummary("Запрос на получение расчасовки")
            .WithDescription("Позволяет получить расчасовку и снятие для предмета по группе, семестру и преподавателю. Возвращает расчасовку и снятие.")
            .WithName("Get Workload")
            .Accepts<GetWorkloadQuery>("application/json")
            .Produces<WorkloadSummaryDTO>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapPost("/save", SaveWorkloadAsync)
            .WithSummary("Запрос на сохранение расчасовки")
            .WithDescription("Позволяет сохранить расчасовку и снятие для предмета по группе, семестру и преподавателю.")
            .WithName("Save Workload")
            .Accepts<SaveWorkloadCommand>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapPost("/finalize", FinalizeDayScheduleAsync)
            .WithSummary("Запрос на сохранение расписания дня")
            .WithDescription("Позволяет сохранять расписание дня для группы на определенный день.")
            .WithName("Finalize Workload")
            .Accepts<FinalizeDayScheduleCommand>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapPost("/draft", SaveDraftScheduleAsync)
            .WithSummary("Запрос на сохранение черновика расписания дня")
            .WithDescription("Позволяет сохранить черновик расписания дня для группы на определенный день. Если день уже существует, его уроки заменяются.")
            .WithName("Save Draft Schedule")
            .Accepts<SaveDraftScheduleCommand>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapGet("/lessons", GetAllLessonsAsync)
            .WithSummary("Запрос на получение информации о предметах")
            .WithDescription("Позволяет получить информацию о предметах, в каком семестре они проводятся, на каком курсу и списку групп.")
            .WithName("Get Lessons Info")
            .Produces<AllLessonsDTO>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapGet("/groups", GetGroupsAsync)
            .WithSummary("Запрос на получение списка групп")
            .WithDescription("Позволяет получить отсортированный список названий групп.")
            .WithName("Get Groups")
            .Produces<List<string>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapGet("/teachers", GetTeachersAsync)
            .WithSummary("Ростер преподавателей")
            .WithDescription("Имена «Фамилия И. О.»; можно отфильтровать по group, lesson и semester.")
            .WithName("Get Teachers")
            .Accepts<GetTeachersQuery>("application/json")
            .Produces<List<string>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

        group.MapGet("/cabinets", GetCabinetsAsync)
            .WithSummary("Кабинеты по категориям")
            .WithDescription("Словарь «Компьютерные»/«Лекционные» со списками имён.")
            .WithName("Get Cabinets")
            .Produces<Dictionary<string, List<string>>>(StatusCodes.Status200OK)
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
    private static async Task<IResult> SaveDraftScheduleAsync(
        [FromServices] IMediator _mediator,
        [FromBody] SaveDraftScheduleCommand command
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
    private static async Task<IResult> GetGroupsAsync(
        [FromServices] IMediator _mediator
    )
    {
        var result = await _mediator.Send(new GetGroupsQuery());

        return result.ToApiResult();
    }
    private static async Task<IResult> GetTeachersAsync(
        [FromServices] IMediator _mediator,
        [AsParameters] GetTeachersQuery query
    )
    {
        var result = await _mediator.Send(query);

        return result.ToApiResult();
    }
    private static async Task<IResult> GetCabinetsAsync(
        [FromServices] IMediator _mediator
    )
    {
        var result = await _mediator.Send(new GetCabinetsQuery());

        return result.ToApiResult();
    }
    }
