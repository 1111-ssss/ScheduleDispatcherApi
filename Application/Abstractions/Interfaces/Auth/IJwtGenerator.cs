using Application.Features.Auth.Login;
using Domain.Abstractions.Result;
using Domain.Entities;

namespace Infrastructure.Abstractions.Interfaces.Auth
{
    public interface IJwtGenerator
    {
        Result<AuthResponse> GenerateToken(User user);
    }
}