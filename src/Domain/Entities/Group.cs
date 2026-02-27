namespace Domain.Entities;

public partial class Group
{
    public string CnG { get; set; } = null!;

    public string? Name { get; set; }

    public int Cours { get; set; }

    public string CnSpec { get; set; } = null!;

    public virtual Specialty CnSpecNavigation { get; set; } = null!;

    public virtual ICollection<Curator> Curators { get; set; } = new List<Curator>();

    public virtual ICollection<SubjectTeacher> SubjectTeachers { get; set; } = new List<SubjectTeacher>();
}
