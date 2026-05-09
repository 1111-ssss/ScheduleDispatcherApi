using Domain.Abstractions.Result;
using FluentValidation;

namespace Application.Features.Dispatcher.FinalizeDaySchedule;

public class FinalizeDayScheduleCommandValidator : AbstractValidator<FinalizeDayScheduleCommand>
{
    public FinalizeDayScheduleCommandValidator()
    {

    }
}
