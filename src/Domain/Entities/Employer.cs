using System;
using System.Collections.Generic;

namespace Infrastructure.src.Domain.Entities;

public partial class Employer
{
    public string CnE { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string FatherName { get; set; } = null!;

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
