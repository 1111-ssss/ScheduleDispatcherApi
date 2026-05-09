using Application.Features.Dispatcher.Common;
using Domain.Model.Result;
using MediatR;

namespace Application.Features.Dispatcher.Commands.SaveDraftSchedule;

public record SaveDraftScheduleCommand(
    string GroupName,
    DateOnly Date,
    List<LessonDraftDTO> Lessons
) : IRequest<Result>;
