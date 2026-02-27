using Domain.Model.Result;
using MediatR;

namespace Application.Features.Dispatcher.FinalizeDaySchedule;

public record FinalizeDayScheduleCommand(
    List<LessonDTO> Lessons,
    DateTime Date,
    string GroupName
) : IRequest<Result>;