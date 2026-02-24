namespace Infrastructure.src.Domain.Entities;

public partial class Specialty
{
    public string CnSpec { get; set; } = null!;

    public string? Name { get; set; }

    public string? Fullname { get; set; }

    public int? Educationperiod { get; set; }

    public string? Department { get; set; }

    public string CnT { get; set; } = null!;

    public virtual Teacher CnTNavigation { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
