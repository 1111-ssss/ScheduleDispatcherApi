using MediatR;
using Application.Features.Auth.Common;
using Domain.Model.Result;

namespace Application.Features.Auth.Login;

public record LoginUserQuery(string Username, string Password) : IRequest<Result<AuthDTO>>;