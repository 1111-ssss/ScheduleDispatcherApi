namespace Application.Abstractions.Interfaces.Auth;

public interface ICurrentUserService
{
    public string? JwtToken { get; }
    public DateTime? ExpiresAt { get; }
}