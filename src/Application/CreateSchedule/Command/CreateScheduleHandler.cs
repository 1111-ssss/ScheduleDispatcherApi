using Contracts.Schedules;
using MassTransit;
using MediatR;

namespace Application.Features.CreateSchedule.Command;

public class CreateScheduleHandler : IRequestHandler<DayScheduleDTO>
{
    private readonly IPublishEndpoint _publishEndpoint; 

    public CreateScheduleHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(DayScheduleDTO request, CancellationToken ct = default)
    {
        Console.WriteLine("хенд dispather");
        await _publishEndpoint.Publish(request, ct);
    }
}
