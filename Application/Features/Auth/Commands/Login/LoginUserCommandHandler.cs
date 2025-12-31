using Application.Abstractions.Interfaces.UnitOfWork;
using Application.Abstractions.Repository;
using Application.Features.Auth.Login;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Auth;
using Infrastructure.Abstractions.Interfaces.Auth;
using MediatR;

namespace Application.Features.Auth.Commands.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserCommandHandler(
        IUnitOfWork uow,
        IJwtGenerator jwtGenerator,
        IBaseRepository<User> userRepository,
        IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<AuthResponse>> Handle(LoginUserCommand request, CancellationToken ct)
    {
        var user = await _userRepository.FirstOrDefaultAsync(new UserByUsernameSpec(request.Username), ct);

        if (user == null)
            return Result<AuthResponse>.Failed(ErrorCode.BadRequest, "Неверный логин или пароль");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash).IsSuccess)
            return Result<AuthResponse>.Failed(ErrorCode.BadRequest, "Неверный email или пароль");

        var result = _jwtGenerator.GenerateToken(user);
        if (!result.IsSuccess)
            return Result<AuthResponse>.Failed(ErrorCode.InternalServerError, "Ошибка генерации токена");

        return Result<AuthResponse>.Success(new AuthResponse
        {
            JwtToken = result.Value,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        });
    }
}