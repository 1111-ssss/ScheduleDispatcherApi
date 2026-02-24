namespace Infrastructure.src.Domain.Entities;

public partial class Classroom
{
    public int IdClassroom { get; set; }

    public int? ClassroomNumber { get; set; }

    public bool? IsPcClassroom { get; set; }

    public virtual ICollection<Subject> IdSubjects { get; set; } = new List<Subject>();
}
