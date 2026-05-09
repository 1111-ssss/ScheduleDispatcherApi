using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class DayScheduleEntity
{
    public int Id { get; set; } = default!;
    public string GroupName { get; set; } = default!;
    public DateOnly Date { get; set; } = default!;
    public List<LessonEntity> Lessons { get; set; } = new();
}
