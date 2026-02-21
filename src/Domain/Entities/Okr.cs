using System;
using System.Collections.Generic;

namespace Infrastructure.src.Domain.Entities;

public partial class Okr
{
    public int IdSchedule { get; set; }

    public virtual Schedule IdScheduleNavigation { get; set; } = null!;
}
