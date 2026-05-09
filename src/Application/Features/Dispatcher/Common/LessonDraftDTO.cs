namespace Application.Features.Dispatcher.Common;

public class LessonDraftDTO
{
    public int LessonNumber { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public string Subject1 { get; init; } = default!;
    public string? Subject2 { get; init; }
    public string Teacher1 { get; init; } = default!;
    public string? Teacher2 { get; init; }
    public string Classroom1 { get; init; } = default!;
    public string? Classroom2 { get; init; }
    public bool Split { get; init; }
}
