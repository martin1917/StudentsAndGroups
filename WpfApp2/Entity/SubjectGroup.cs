namespace WpfApp2.Entity;

public class SubjectGroup : BaseEntity
{
    public int SubjectId { get; set; }

    public int NumGroup { get; set; }

    public Subject Subject { get; set; }
}
