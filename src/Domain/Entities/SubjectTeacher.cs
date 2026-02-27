namespace Domain.Entities;

public partial class SubjectTeacher
{
    public int IdsubjectTeacher { get; set; }

    public int IdSubject { get; set; }

    public string CnG { get; set; } = null!;

    public string CnT { get; set; } = null!;

    public virtual Group CnGNavigation { get; set; } = null!;

    public virtual Teacher CnTNavigation { get; set; } = null!;

    public virtual Subject IdSubjectNavigation { get; set; } = null!;

    public virtual ICollection<SubjectTeacherSchedule> SubjectTeacherSchedules { get; set; } = new List<SubjectTeacherSchedule>();

    public virtual ICollection<SubjectTeacherSemester> Subjectteachersemesters { get; set; } = new List<SubjectTeacherSemester>();
}
