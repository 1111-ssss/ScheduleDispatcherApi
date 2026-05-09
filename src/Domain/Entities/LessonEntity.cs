using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class LessonEntity
{
    public int Groupid { get; set; } = default!;
    public string Subject1 { get; set; } = default!;
    public string Subject2 { get; set; } = default!;
    public string Teacher1 { get; set; } = default!;
    public string Teacher2 { get; set; } = default!;
    public string Classroom1 { get; set; } = default!;
    public string Classroom2 { get; set; } = default!;
    public TimeOnly StartTime { get; set; } = default!;
    public TimeOnly EndTime { get; set; } = default!;
    public int LessonNumber { get; set; } = default!;
    public bool Split { get; set; }
    public DayScheduleEntity daySchedule { get; set; } = default!;
}
