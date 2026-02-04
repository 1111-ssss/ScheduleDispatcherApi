using Domain.Abstractions.Result;

namespace Infrastructure.Abstractions.Interfaces.Auth;

public interface IPasswordHasher
{
    Result<string> Hash(string password);
    Result Verify(string password, string hash);
}