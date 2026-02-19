using Domain.Abstractions.Result;
using FluentValidation;

namespace Application.Features.Dispatcher.FinalizeDaySchedule;

public class FinalizeDayScheduleCommandValidator : AbstractValidator<FinalizeDayScheduleCommand>
{
    public FinalizeDayScheduleCommandValidator()
    {
        RuleFor(x => x.GroupName)
            .NotEmpty().WithState(_ => ErrorCode.InvalidGroupName)
            .WithMessage("Поле {PropertyName} не может быть пустым")
            .Matches(@"^[А-Я]\d{2}$").WithState(_ => ErrorCode.InvalidGroupName)
            .WithMessage("Поле {PropertyName} не соответствует формату");

        RuleFor(x => x.Date)
            .NotEmpty().WithState(_ => ErrorCode.InvalidDate)
            .WithMessage("Поле {PropertyName} не может быть пустым")
            .Must(x => x.Date >= DateTime.Today).WithState(_ => ErrorCode.InvalidDate)
            .WithMessage("Поле {PropertyName} должно быть больше или равно сегодняшнему дню");
    }
}
