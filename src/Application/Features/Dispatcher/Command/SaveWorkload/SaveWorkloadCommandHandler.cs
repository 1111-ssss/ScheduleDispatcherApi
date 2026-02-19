using Domain.Abstractions.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Dispatcher.SaveWorkload;

public class SaveWorkloadCommandHandler : IRequestHandler<SaveWorkloadCommand, Result>
{
    private readonly ILogger<SaveWorkloadCommandHandler> _logger;

    public SaveWorkloadCommandHandler(
        ILogger<SaveWorkloadCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<Result> Handle(SaveWorkloadCommand command, CancellationToken ct)
    {
        throw new NotImplementedException("Save workload еще не реализован");

        // return Result.Success();
    }
}