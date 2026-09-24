using Domain.Abstractions.Result;
using FluentValidation;

namespace Application.Features.Dispatcher.Commands.SaveDraftSchedule;

public class SaveDraftScheduleCommandValidator : AbstractValidator<SaveDraftScheduleCommand>
{
    public SaveDraftScheduleCommandValidator()
    {
        RuleFor(x => x.GroupName)
            .NotEmpty().WithState(_ => ErrorCode.InvalidGroupName)
            .WithMessage("Поле {PropertyName} не может быть пустым")
            .MaximumLength(50).WithState(_ => ErrorCode.InvalidGroupName)
            .WithMessage("Поле {PropertyName} должно быть не больше 50 символов");

        RuleFor(x => x.Date)
            .NotEmpty().WithState(_ => ErrorCode.InvalidDate)
            .WithMessage("Поле {PropertyName} не может быть пустым")
            .Must(x => x >= DateOnly.FromDateTime(DateTime.Today)).WithState(_ => ErrorCode.InvalidDate)
            .WithMessage("Поле {PropertyName} должно быть больше или равно сегодняшнему дню");

        RuleFor(x => x.Lessons)
            .NotEmpty().WithState(_ => ErrorCode.InvalidWorkload)
            .WithMessage("Поле {PropertyName} не может быть пустым");

        RuleForEach(x => x.Lessons)
            .ChildRules(lesson =>
            {
                lesson.RuleFor(x => x.LessonNumber)
                    .GreaterThan(0).WithState(_ => ErrorCode.InvalidWorkload)
                    .WithMessage("Поле {PropertyName} должно быть больше 0");
                lesson.RuleFor(x => x.StartTime)
                    .NotEmpty().WithState(_ => ErrorCode.InvalidWorkload)
                    .WithMessage("Поле {PropertyName} не может быть пустым");
                lesson.RuleFor(x => x.EndTime)
                    .NotEmpty().WithState(_ => ErrorCode.InvalidWorkload)
                    .WithMessage("Поле {PropertyName} не может быть пустым")
                    .GreaterThan(x => x.StartTime).WithState(_ => ErrorCode.InvalidWorkload)
                    .WithMessage("Поле {PropertyName} должно быть больше времени начала");
                lesson.RuleFor(x => x.Subject1)
                    .NotEmpty().WithState(_ => ErrorCode.InvalidWorkload)
                    .WithMessage("Поле {PropertyName} не может быть пустым");
                lesson.RuleFor(x => x.Teacher1)
                    .NotEmpty().WithState(_ => ErrorCode.InvalidWorkload)
                    .WithMessage("Поле {PropertyName} не может быть пустым");
                lesson.RuleFor(x => x.Classroom1)
                    .NotEmpty().WithState(_ => ErrorCode.InvalidWorkload)
                    .WithMessage("Поле {PropertyName} не может быть пустым");
            });
    }
}
