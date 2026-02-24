namespace Infrastructure.src.Domain.Entities;

public partial class Removal
{
    public int IdRemoval { get; set; }

    public int IdSchedule1 { get; set; }

    public int IdSchedule2 { get; set; }

    public virtual Schedule IdSchedule1Navigation { get; set; } = null!;

    public virtual Schedule IdSchedule2Navigation { get; set; } = null!;
}
