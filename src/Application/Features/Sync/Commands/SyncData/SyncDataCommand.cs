using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Sync.SyncData;

public record SyncDataCommand() : IRequest<Result>;