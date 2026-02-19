using Application.Features.Auth.Common;
using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Auth.Refresh;

public record RefreshTokenQuery() : IRequest<Result<AuthDTO>>;