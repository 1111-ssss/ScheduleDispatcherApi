using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Domain.Abstractions.Result;
using Infrastructure.Abstractions.Interfaces.Auth;

namespace Infrastructure.Services.Auth;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltLength = 16;
    private const int HashLength = 32;
    private const int MemoryCostKb = 64 * 1024;
    private const int Iterations = 3;
    private const int Parallelism = 4;
    public Result<string> Hash(string password)
    {
        try
        {
            var salt = RandomNumberGenerator.GetBytes(SaltLength);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var argon2 = new Argon2id(passwordBytes)
            {
                Salt = salt,
                DegreeOfParallelism = Parallelism,
                MemorySize = MemoryCostKb,
                Iterations = Iterations
            };
            var hash = argon2.GetBytes(HashLength);

            var result = Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            return Result<string>.Success(result);
        }
        catch
        {
            return Result<string>.Failed(ErrorCode.InternalServerError, "Ошибка хеширования");
        }
    }
    public Result Verify(string password, string hash)
    {
        if (string.IsNullOrEmpty(hash))
            return Result.Failed(ErrorCode.InvalidUsernameOrPassword, "Хеш пустой");

        try
        {
            var parts = hash.Split(':');
            if (parts.Length != 2)
                return Result.Failed(ErrorCode.InternalServerError, "Хеш не в формате salt:hash");

            var salt = Convert.FromBase64String(parts[0]);
            var hashBytes = Convert.FromBase64String(parts[1]);
            var pwdBytes = Encoding.UTF8.GetBytes(password);
            var argon2 = new Argon2id(pwdBytes)
            {
                Salt = salt,
                DegreeOfParallelism = Parallelism,
                MemorySize = MemoryCostKb,
                Iterations = Iterations
            };
            var computed = argon2.GetBytes(hashBytes.Length);
            if (CryptographicOperations.FixedTimeEquals(computed, hashBytes))
                return Result.Success();

            return Result.Failed(ErrorCode.InvalidUsernameOrPassword, "Пароль либо логин неверный");
        }
        catch
        {
            return Result.Failed(ErrorCode.InternalServerError, "Пароль либо логин неверный");
        }
    }
}