namespace Domain.Entities;

public partial class Teacher
{
    public string CnT { get; set; } = null!;

    public int? Idcategory { get; set; }

    public int? Idck { get; set; }

    public string CnE { get; set; } = null!;

    public virtual Employer CnENavigation { get; set; } = null!;

    public virtual ICollection<Curator> Curators { get; set; } = new List<Curator>();

    public virtual ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();

    public virtual ICollection<SubjectTeacher> SubjectTeachers { get; set; } = new List<SubjectTeacher>();
}
