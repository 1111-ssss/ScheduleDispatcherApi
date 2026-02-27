using Application.Abstractions.Interfaces.Auth;
using Application.Features.Auth.Common;
using Domain.Abstractions.Result;
using Domain.Model.Result;
using Infrastructure.Abstractions.Interfaces.Auth;
using MediatR;

namespace Application.Features.Auth.Refresh;

public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, Result<AuthDTO>>
{
    private readonly IJwtGenerator _jwtGenerator;
    private readonly ICurrentUserService _currentUserService;

    public RefreshTokenQueryHandler(
        IJwtGenerator jwtGenerator,
        ICurrentUserService currentUserService
    )
    {
        _jwtGenerator = jwtGenerator;
        _currentUserService = currentUserService;
    }

    public async Task<Result<AuthDTO>> Handle(RefreshTokenQuery request, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(_currentUserService.JwtToken))
            return Result.Failed(ErrorCode.InvalidToken);

        if (DateTime.UtcNow > _currentUserService.ExpiresAt)
            return Result.Failed(ErrorCode.TokenExpired);

        var currentToken = _currentUserService.TokenDTO;
        if (currentToken == null)
            return Result.Failed(ErrorCode.InvalidToken);

        var newJwtToken = _jwtGenerator.GenerateToken(
            currentToken
        );
        if (string.IsNullOrEmpty(newJwtToken))
            return Result<AuthDTO>.Failed(ErrorCode.TokenGenerationError);

        return Result<AuthDTO>.Success(new AuthDTO(
            JwtToken: newJwtToken
        ));
    }
}