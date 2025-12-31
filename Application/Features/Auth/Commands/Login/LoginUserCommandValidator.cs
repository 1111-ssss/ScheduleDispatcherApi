using Application.Features.Auth.Login;
using FluentValidation;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Логин обязателен")
                .MinimumLength(3).WithMessage("Логин должен быть не менее 3 символов")
                .MaximumLength(30).WithMessage("Логин не должен превышать 30 символов")
                .Matches(@"^[a-zA-Z0-9]+$")
                .WithMessage("Логин может содержать только буквы и цифры");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Пароль должен быть не менее 6 символов")
                .MaximumLength(50).WithMessage("Пароль не должен превышать 50 символов")
                .Matches(@"^[\w!@#$%^&*()_+\-=\[\]{};':\\|,\.<>/?~`]+$")
                .WithMessage("Пароль содержит недпустимые символы");
        }
    }
}