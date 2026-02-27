namespace Domain.Entities;

public partial class SubjectTeacherSchedule
{
    public int IdsubjectTeacher { get; set; }

    public int? PracticalHours2term { get; set; }

    public bool? GroupSplit { get; set; }

    public int? LectureHours1term { get; set; }

    public int? LectureHours2term { get; set; }

    public int? PracticalHours1term { get; set; }

    public int? ExaminationHours { get; set; }

    public int IdSchedule { get; set; }

    public virtual Schedule IdScheduleNavigation { get; set; } = null!;

    public virtual SubjectTeacher IdsubjectTeacherNavigation { get; set; } = null!;
}
