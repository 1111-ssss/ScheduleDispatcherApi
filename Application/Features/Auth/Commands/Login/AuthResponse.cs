namespace Application.Features.Auth.Login
{
    public record AuthResponse(string JwtToken = default!, DateTime ExpiresAt = default!);
}