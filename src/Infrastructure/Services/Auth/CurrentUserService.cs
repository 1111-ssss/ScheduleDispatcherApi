using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Abstractions.Interfaces.Auth;
using Application.Abstractions.Model.DTO;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Auth;

public class CurrentUserService : ICurrentUserService
    {
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string? JwtToken => _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
    public DateTime? ExpiresAt => GetExpirationFromClaims();
    public GenerateTokenDTO? TokenDTO => GetTokenDTO();

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    private DateTime? GetExpirationFromClaims()
    {
        if (User == null || !User.Identity?.IsAuthenticated == true)
            return null;

        var expClaim = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
        if (expClaim != null && long.TryParse(expClaim, out var expSeconds))
        {
            return DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
        }
        return null;
    }
    private GenerateTokenDTO? GetTokenDTO()
    {
        if (User == null || !User.Identity?.IsAuthenticated == true)
            return null;

        var idClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        var usernameClaim = User.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value
            ?? User.FindFirst(ClaimTypes.Name)?.Value;
        var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

        if (idClaim == null || usernameClaim == null || roleClaim == null)
            return null;

        if (int.TryParse(idClaim, out var userId))
            return new GenerateTokenDTO(userId, usernameClaim, roleClaim);

        return null;
    }
    }
