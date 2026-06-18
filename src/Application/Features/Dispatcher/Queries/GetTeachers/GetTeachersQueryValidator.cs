using Domain.Abstractions.Result;
using FluentValidation;

namespace Application.Features.Dispatcher.GetTeachers;

public class GetTeachersQueryValidator : AbstractValidator<GetTeachersQuery>
    {
    public GetTeachersQueryValidator()
    {
        When(x => !string.IsNullOrWhiteSpace(x.Group), () =>
        {
            RuleFor(x => x.Group!)
                .MaximumLength(50).WithState(_ => ErrorCode.InvalidGroupName)
                .WithMessage("Поле {PropertyName} должно быть не больше 50 символов")
                .Matches("^[А-Я]\\d{2}$").WithState(_ => ErrorCode.InvalidGroupName)
                .WithMessage("Поле {PropertyName} должно быть в формате «Буква + 2 цифры»");
        });
    }
    }
