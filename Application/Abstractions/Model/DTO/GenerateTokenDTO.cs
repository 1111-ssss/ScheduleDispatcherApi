using Domain.Entities;

namespace Application.Abstractions.Model.DTO;

public record GenerateTokenDTO(
    int Id,
    string Username,
    string Role
)
{
    public static GenerateTokenDTO FromUser(User user) => new(user.Id, user.Username, user.Role);
}