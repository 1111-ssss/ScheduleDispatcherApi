using Application.Abstractions.Repository.Custom;
using Application.Features.Dispatcher.Commands.SaveDraftSchedule;
using Domain.Entities;
using Domain.Model.Result;
using MediatR;

namespace Application.Features.ScheduleDraft.Handlers;

public class SaveDraftScheduleCommandHandler : IRequestHandler<SaveDraftScheduleCommand, Result>
{
    private readonly IScheduleDraftRepository _repository;

    public SaveDraftScheduleCommandHandler(IScheduleDraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(SaveDraftScheduleCommand command, CancellationToken ct)
    {
        var lessons = command.Lessons.Select(l => new LessonEntity
        {
            LessonNumber = l.LessonNumber,
            StartTime = l.StartTime,
            EndTime = l.EndTime,
            Subject1 = l.Subject1,
            Subject2 = l.Subject2,
            Teacher1 = l.Teacher1,
            Teacher2 = l.Teacher2,
            Classroom1 = l.Classroom1,
            Classroom2 = l.Classroom2,
            Split = l.Split
        }).ToList();

        await _repository.SaveDraftAsync(command.GroupName, command.Date, lessons, ct);
        return Result.Success();
    }
}
