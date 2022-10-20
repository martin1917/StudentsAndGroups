using System;
using WpfApp2.ViewModels.Base;

namespace WpfApp2.Models;

/// <summary> Класс, характеризующий успеваемость ученика (Отображение на UI) </summary>
public class AcademicPerformanceLogModel : BaseViewModel
{
    private int _id;
    private int _subjectId;
    private int _groupId;
    private int _studentId;
    private DateOnly _date;
    private int? _mark;
    private SubjectModel _subjectModel;
    private GroupModel _groupModel;
    private StudentModel _studentModel;

    public int Id { get => _id; set => Set(ref _id, value); }

    /// <summary> ID учебного предмета </summary>
    public int SubjectId { get => _subjectId; set => Set(ref _subjectId, value); }

    /// <summary> ID учебной группы </summary>
    public int GroupId { get => _groupId; set => Set(ref _groupId, value); }

    /// <summary> ID ученика </summary>
    public int StudentId { get => _studentId; set => Set(ref _studentId, value); }

    /// <summary> Дата, за которую была поставлена оценка </summary>
    public DateOnly Date { get => _date; set => Set(ref _date, value); }

    /// <summary> Оценка (2, 3, 4, 5, null - пропуск) </summary>
    public int? Mark { get => _mark; set => Set(ref _mark, value); }

    /// <summary> Учебный предмет </summary>
    public SubjectModel SubjectModel { get => _subjectModel; set => Set(ref _subjectModel, value); }

    /// <summary> Группа </summary>
    public GroupModel GroupModel { get => _groupModel; set => Set(ref _groupModel, value); }

    /// <summary> Студент </summary>
    public StudentModel StudentModel { get => _studentModel; set => Set(ref _studentModel, value); }
}
