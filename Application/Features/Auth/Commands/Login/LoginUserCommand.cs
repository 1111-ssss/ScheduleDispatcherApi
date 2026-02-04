using MediatR;
using Domain.Abstractions.Result;

namespace Application.Features.Auth.Login;

public record LoginUserCommand(string Username, string Password) : IRequest<Result<AuthResponse>>;