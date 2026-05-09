using Contracts.Common;
using MediatR;

namespace Contracts.Schedules;

public class DayScheduleDTO : IRequest
{
    public string Group {  get; init; }
    public DateOnly Date { get; init; }
    public List<Lesson> Lessons { get; init; } 
}
