using Application.Abstractions.Repository.Custom;
using Application.Features.Dispatcher.FinalizeDaySchedule;
using Contracts.Schedules;
using Domain.Abstractions.Result;
using Contracts.Common;
using Domain.Model.Result;
using MassTransit;
using MediatR;

namespace Application.Features.ScheduleFinalize.Handlers;

public class FinalizeDayScheduleCommandHandler : IRequestHandler<FinalizeDayScheduleCommand, Result>
{
    private readonly IScheduleDraftRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public FinalizeDayScheduleCommandHandler(
        IScheduleDraftRepository repository,
        IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result> Handle(FinalizeDayScheduleCommand command, CancellationToken ct)
    {
        var daySchedule = await _repository.GetWithLessonsAsync(command.GroupName, command.Date, ct);

        if (daySchedule == null || !daySchedule.Lessons.Any())
            return Result.FailedOperation(ErrorCode.NotFound);

        var dto = new DayScheduleDTO
        {
            Group = daySchedule.GroupName,
            Date = daySchedule.Date,
            Lessons = daySchedule.Lessons
                .OrderBy(l => l.StartTime)
                .Select(l => new Lesson
                {
                    StartTime = l.StartTime,
                    EndTime = l.EndTime,
                    Lesson1 = l.Subject1,
                    Lesson2 = l.Subject2,
                    Fio1 = l.Teacher1,
                    Fio2 = l.Teacher2,
                    ClassRoom1 = l.Classroom1,
                    ClassRoom2 = l.Classroom2
                }).ToList()
        };

        await _publishEndpoint.Publish(dto, ct);
        return Result.Success();
    }
}
