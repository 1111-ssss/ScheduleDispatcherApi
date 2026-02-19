using Domain.Abstractions.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Dispatcher.FinalizeDaySchedule;

public class FinalizeDayScheduleCommandHandler : IRequestHandler<FinalizeDayScheduleCommand, Result>
{
    private readonly ILogger<FinalizeDayScheduleCommandHandler> _logger;

    public FinalizeDayScheduleCommandHandler(
        ILogger<FinalizeDayScheduleCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<Result> Handle(FinalizeDayScheduleCommand command, CancellationToken ct)
    {
        throw new NotImplementedException("Finalize Day Schedule еще не реализована");

        // return Result.Success();
    }
}