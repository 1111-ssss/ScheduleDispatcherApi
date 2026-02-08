using Application.Abstractions.Model.DTO;

namespace Infrastructure.Abstractions.Interfaces.Auth;

public interface IJwtGenerator
{
    string? GenerateToken(GenerateTokenDTO dto);
}