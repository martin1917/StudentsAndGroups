namespace WpfApp2.State;

/// <summary> Типы VM </summary>
public enum ViewModelType
{
    /// <summary> главная VM </summary>
    Main,

    /// <summary> VM с группами и студентами </summary>
    GroupsStudents,

    /// <summary> VM с оценками </summary>
    AcademyJournal,

    /// <summary> VM со всеми предметами </summary>
    AllSubjects,

    /// <summary> VM с предметами для группы </summary>
    SubjectsForGroup,

    /// <summary> VM со средними оценками </summary>
    AvgMarks,

    /// <summary> VM для другого окна </summary>
    Other
}
