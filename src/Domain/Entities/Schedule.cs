namespace Domain.Entities;

public partial class Schedule
{
    public int IdSchedule { get; set; }

    public int? Lessonnumber { get; set; }

    public DateTime? Date1 { get; set; }

    public DateTime? Date2 { get; set; }

    public bool? IsPractical { get; set; }

    public bool? IsOver { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual Okr? Okr { get; set; }

    public virtual Removal? RemovalAsFirst { get; set; }
    
    public virtual Removal? RemovalAsSecond { get; set; }

    public virtual ICollection<SubjectTeacherSchedule> SubjectTeacherSchedules { get; set; } = new List<SubjectTeacherSchedule>();
}
