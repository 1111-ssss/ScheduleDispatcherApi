using Application.Features.Auth.Common;
using Domain.Model.Result;
using MediatR;

namespace Application.Features.Auth.Refresh;

public record RefreshTokenQuery() : IRequest<Result<AuthDTO>>;