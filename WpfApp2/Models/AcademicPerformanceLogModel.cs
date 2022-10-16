using System;
using WpfApp2.ViewModels;

namespace WpfApp2.Models;

public class AcademicPerformanceLogModel : BaseViewModel
{
    private int _id;
    public int Id { get => _id; set => Set(ref _id, value); }

    private int _subjectId;
    public int SubjectId { get => _subjectId; set => Set(ref _subjectId, value); }

    private int _groupId;
    public int GroupId { get => _groupId; set => Set(ref _groupId, value); }

    private int _studentId;
    public int StudentId { get => _studentId; set => Set(ref _studentId, value); }

    /// <summary> Дата, за которую была поставлена оценка </summary>
    private DateOnly _date;
    public DateOnly Date { get => _date; set => Set(ref _date, value); }

    /// <summary> Оценка (2, 3, 4, 5, null - пропуск) </summary>
    private int? _mark;
    public int? Mark { get => _mark; set => Set(ref _mark, value); }

    private SubjectModel _subjectModel;
    public SubjectModel SubjectModel { get => _subjectModel; set => Set(ref _subjectModel, value); }

    /// <summary> Группа </summary>
    private GroupModel _groupModel;
    public GroupModel GroupModel { get => _groupModel; set => Set(ref _groupModel, value); }

    /// <summary> Студент </summary>
    private StudentModel _studentModel;
    public StudentModel StudentModel { get => _studentModel; set => Set(ref _studentModel, value); }
}
