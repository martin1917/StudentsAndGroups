namespace WpfApp2.Entity;

/// <summary> Класс, показывающий какой предмет в каком классе изучается </summary>
public class SubjectGroup
{
    public int Id { get; set; }

    /// <summary> ID учебного предмета </summary>
    public int SubjectId { get; set; }

    /// <summary> Номер класса (5 класс, 8 класс, т.д.) </summary>
    public int NumGroup { get; set; }

    /// <summary> Учебный предмет </summary>
    public Subject Subject { get; set; }
}
