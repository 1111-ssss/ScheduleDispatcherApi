using Application.Features.Dispatcher.Common;
using Application.Features.Dispatcher.GetWorkload;
using Domain.Abstractions.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Dispatcher.SaveWorkload;

public class GetWorkloadQueryHandler : IRequestHandler<GetWorkloadQuery, Result<WorkloadSummaryDTO>>
{
    private readonly ILogger<GetWorkloadQueryHandler> _logger;

    public GetWorkloadQueryHandler(
        ILogger<GetWorkloadQueryHandler> logger)
    {
        _logger = logger;
    }

    public async Task<Result<WorkloadSummaryDTO>> Handle(GetWorkloadQuery query, CancellationToken ct)
    {
        throw new NotImplementedException("Get workload еще не реализован");

        // return Result.Success();
    }
}