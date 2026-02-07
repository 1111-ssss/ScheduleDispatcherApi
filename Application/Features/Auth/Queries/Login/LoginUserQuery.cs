using MediatR;
using Domain.Abstractions.Result;
using Application.Features.Auth.Common;

namespace Application.Features.Auth.Login;

public record LoginUserQuery(string Username, string Password) : IRequest<Result<AuthResponse>>;