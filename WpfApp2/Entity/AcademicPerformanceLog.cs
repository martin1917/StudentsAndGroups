using System;

namespace WpfApp2.Entity;

/// <summary> Класс, характеризующий успеваемость ученика </summary>
public class AcademicPerformanceLog
{
    public int Id { get; set; }

    /// <summary> ID учебного предмета </summary>
    public int SubjectId { get; set; }

    /// <summary> ID учебной группы </summary>
    public int GroupId { get; set; }

    /// <summary> ID ученика </summary>
    public int StudentId { get; set; }

    /// <summary> Дата, за которую была поставлена оценка </summary>
    public DateOnly Date{ get; set; }

    /// <summary> Оценка (2, 3, 4, 5, null - пропуск) </summary>
    public int? Mark { get; set; }

    /// <summary> Учебный предмет </summary>
    public Subject Subject { get; set; }

    /// <summary> Учебная группа </summary>
    public Group Group { get; set; }

    /// <summary> Студент </summary>
    public Student Student { get; set; }
}
