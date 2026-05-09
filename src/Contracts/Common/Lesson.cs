//using Domain.Common.Enums;

namespace Contracts.Common;

public class Lesson
{
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public string Lesson1 { get; init; }
    public string Lesson2 { get; init; }
    public string Fio1 { get; init; }
    public string Fio2 { get; init; }
    public string ClassRoom1 { get; init; }
    public string ClassRoom2 { get; init; }
    //public durationState DurationState { get; set; }
}
