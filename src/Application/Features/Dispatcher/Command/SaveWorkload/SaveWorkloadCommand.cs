using Application.Features.Dispatcher.Common;
using Domain.Abstractions.Result;
using MediatR;

namespace Application.Features.Dispatcher.SaveWorkload;

public record SaveWorkloadCommand(
    WorkloadSummaryDTO WorkloadSummary
) : IRequest<Result>;