using System;
using System.Collections.Generic;

namespace Infrastructure.src.Domain.Entities;

public partial class Subject
{
    public int IdSubject { get; set; }

    public string Name { get; set; } = null!;

    public string? Fullname { get; set; }

    public bool? Optional { get; set; }

    public int Totalhourcount { get; set; }

    public int Practichourcount { get; set; }

    public bool PcClassNeed { get; set; }

    public string CnSpec { get; set; } = null!;

    public virtual Specialty CnSpecNavigation { get; set; } = null!;

    public virtual ICollection<SubjectTeacher> SubjectTeachers { get; set; } = new List<SubjectTeacher>();

    public virtual ICollection<Classroom> IdClassrooms { get; set; } = new List<Classroom>();
}
