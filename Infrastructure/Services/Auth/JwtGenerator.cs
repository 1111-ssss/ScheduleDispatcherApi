using Application.Features.Auth.Login;
using Domain.Abstractions.Result;
using Domain.Entities;
using Infrastructure.Abstractions.Interfaces.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services.Auth;

public class JwtGenerator : IJwtGenerator
{
    private readonly IConfiguration _config;
    private readonly string _key;
    public JwtGenerator(IConfiguration config)
    {
        _config = config;
        _key = _config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key пустой в конфигурации");
    }
    public Result<AuthResponse> GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_config["Jwt:Expires"] ?? "15")
            ),
            audience: _config["Jwt:Audience"],
            issuer: _config["Jwt:Issuer"],
            signingCredentials: creds
        );

        return Result<AuthResponse>.Success(new AuthResponse {
            JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                (token.ValidTo - token.ValidFrom).TotalMinutes
            )
        });
    }
}