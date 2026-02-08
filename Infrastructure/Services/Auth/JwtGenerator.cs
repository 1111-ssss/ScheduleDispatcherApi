using Application.Abstractions.Model.DTO;
using Infrastructure.Abstractions.Interfaces.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Auth;

public class JwtGenerator : IJwtGenerator
{
    private readonly IConfiguration _config;
    private readonly ILogger<JwtGenerator> _logger;
    private readonly string _key;
    public JwtGenerator(
        IConfiguration config,
        ILogger<JwtGenerator> logger
    )
    {
        _config = config;
        _logger = logger;
        _key = _config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key пустой в конфигурации");
    }
    public string? GenerateToken(GenerateTokenDTO dto)
    {
        try {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, dto.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, dto.Username),
                new Claim(ClaimTypes.Role, dto.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["Jwt:Expires"] ?? "30")
                ),
                audience: _config["Jwt:Audience"],
                issuer: _config["Jwt:Issuer"],
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Ошибка при генерации JWT токена");
            return null;
        }
    }
}