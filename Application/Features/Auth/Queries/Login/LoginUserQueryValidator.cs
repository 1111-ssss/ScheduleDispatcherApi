using Domain.Abstractions.Result;
using FluentValidation;

namespace Application.Features.Auth.Login;

public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
{
    public LoginUserQueryValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithState(_ => ErrorCode.InvalidUsername)
            .WithMessage("Поле {PropertyName} не может быть пустым")
            .MinimumLength(3).WithState(_ => ErrorCode.InvalidUsername)
            .WithMessage("Поле {PropertyName} должно содержать не менее 3 символов")
            .MaximumLength(30).WithState(_ => ErrorCode.InvalidUsername)
            .WithMessage("Поле {PropertyName} должно содержать не более 30 символов")
            .Matches(@"^[a-zA-Z0-9]+$").WithState(_ => ErrorCode.InvalidUsername)
            .WithMessage("Поле {PropertyName} должно содержать только буквы и цифры");

        RuleFor(x => x.Password)
            .NotEmpty().WithState(_ => ErrorCode.InvalidPassword)
            .WithMessage("Поле {PropertyName} не может быть пустым")
            .MinimumLength(3).WithState(_ => ErrorCode.InvalidPassword)
            .WithMessage("Поле {PropertyName} должно содержать не менее 3 символов")
            .MaximumLength(50).WithState(_ => ErrorCode.InvalidPassword)
            .WithMessage("Поле {PropertyName} должно содержать не более 50 символов")
            .Matches(@"^[\w!@#$%^&*()_+\-=\[\]{};':\\|,\.<>/?~`]+$").WithState(_ => ErrorCode.InvalidPassword)
            .WithMessage("Поле {PropertyName} должно содержать только буквы, цифры, знаки препинания, специальные символы и символы подчеркивания");
    }
}
