namespace Infrastructure.src.Domain.Entities;

public partial class Exam
{
    public int IdSchedule { get; set; }

    public virtual Schedule IdScheduleNavigation { get; set; } = null!;
}
