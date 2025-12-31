using Domain.Abstractions.Result;
using Domain.Entities;

namespace Infrastructure.Abstractions.Interfaces.Auth
{
    public interface IJwtGenerator
    {
        Result<string> GenerateToken(User user);
    }
}