using Domain.Model.Result;
using MediatR;

namespace Application.Features.Dispatcher.FinalizeDaySchedule;

public record FinalizeDayScheduleCommand(
    string GroupName,
    DateOnly Date
) : IRequest<Result>;
