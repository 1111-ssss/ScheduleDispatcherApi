using Domain.Abstractions.Result;
using FluentValidation;

namespace Application.Features.Dispatcher.GetWorkload;

public class GetWorkloadQueryValidator : AbstractValidator<GetWorkloadQuery>
{
    public GetWorkloadQueryValidator()
    {
        RuleFor(x => x.Lesson)
            .NotEmpty().WithState(_ => ErrorCode.InvalidLessonName)
            .WithMessage("Поле {PropertyName} не может быть пустым")
            .MaximumLength(100).WithState(_ => ErrorCode.InvalidLessonName)
            .WithMessage("Поле {PropertyName} должно быть не больше 100 символов");
        RuleFor(x => x.Teacher)
            .NotEmpty().WithState(_ => ErrorCode.InvalidTeacherName)
            .WithMessage("Поле {PropertyName} не может быть пустым")
            .MaximumLength(100).WithState(_ => ErrorCode.InvalidTeacherName)
            .WithMessage("Поле {PropertyName} должно быть не больше 100 символов");
        RuleFor(x => x.Group)
            .NotEmpty().WithState(_ => ErrorCode.InvalidGroupName)
            .WithMessage("Поле {PropertyName} не может быть пустым")
            .Matches(@"^[А-Я]\d{2}$").WithState(_ => ErrorCode.InvalidGroupName)
            .WithMessage("Поле {PropertyName} не соответствует формату");
        RuleFor(x => x.Semester)
            .NotEmpty().WithState(_ => ErrorCode.InvalidSemester)
            .WithMessage("Поле {PropertyName} не может быть пустым")
            .GreaterThan(0).WithState(_ => ErrorCode.InvalidSemester)
            .WithMessage("Поле {PropertyName} должно быть больше 0");
    }
}
