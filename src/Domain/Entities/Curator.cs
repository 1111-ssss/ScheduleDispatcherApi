namespace Domain.Entities;

public partial class Curator
{
    public int IdCurator { get; set; }

    public bool Primary { get; set; }

    public string CnT { get; set; } = null!;

    public string CnG { get; set; } = null!;

    public virtual Group CnGNavigation { get; set; } = null!;

    public virtual Teacher CnTNavigation { get; set; } = null!;
}
