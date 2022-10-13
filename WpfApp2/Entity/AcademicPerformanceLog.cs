using System;

namespace WpfApp2.Entity;

public class AcademicPerformanceLog : BaseEntity
{
    public int SubjectId { get; set; }

    public int GroupId { get; set; }

    public int StudentId { get; set; }

    /// <summary> Дата, за которую была поставлена оценка </summary>
    public DateOnly Date{ get; set; }

    /// <summary> Оценка (2, 3, 4, 5, null - пропуск) </summary>
    public int? Mark { get; set; }

    /// <summary> Предмет </summary>
    public Subject Subject { get; set; }

    /// <summary> Группа </summary>
    public Group Group { get; set; }

    /// <summary> Студент </summary>
    public Student Student { get; set; }
}
