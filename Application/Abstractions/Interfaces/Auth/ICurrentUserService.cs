namespace Application.Abstractions.Interfaces.Auth;

public interface ICurrentUserService
{
    public string? JwtToken { get; }
}