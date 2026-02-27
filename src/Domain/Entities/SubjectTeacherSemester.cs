namespace Domain.Entities;

public partial class SubjectTeacherSemester
{
    public int IdsubjectTeacherSemester { get; set; }

    public bool? Semester1 { get; set; }

    public bool? Semester2 { get; set; }

    public int IdsubjectTeacher { get; set; }

    public virtual SubjectTeacher IdsubjectTeacherNavigation { get; set; } = null!;
}
