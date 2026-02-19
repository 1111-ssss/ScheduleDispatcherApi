using Application.Abstractions.Model.DTO;
using Application.Abstractions.Repository;
using Application.Features.Auth.Common;
using Domain.Abstractions.Result;
using Domain.Entities;
using Domain.Specifications.Auth;
using Infrastructure.Abstractions.Interfaces.Auth;
using MediatR;

namespace Application.Features.Auth.Login;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<AuthDTO>>
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserQueryHandler(
        IBaseRepository<User> userRepository,
        IJwtGenerator jwtGenerator,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtGenerator = jwtGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<AuthDTO>> Handle(LoginUserQuery request, CancellationToken ct)
    {
        var user = await _userRepository.FirstOrDefaultAsync(new UserByUsernameSpec(request.Username), ct);

        if (user == null)
            return Result.Failed(ErrorCode.InvalidUsernameOrPassword);

        var hashResult = _passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!hashResult.IsSuccess)
            return hashResult;

        var userJwt = _jwtGenerator.GenerateToken(
            GenerateTokenDTO.FromUser(user)
        );
        
        if (string.IsNullOrEmpty(userJwt))
            return Result<AuthDTO>.Failed(ErrorCode.TokenGenerationError);

        return Result<AuthDTO>.Success(new AuthDTO(
            JwtToken: userJwt
        ));
    }
}