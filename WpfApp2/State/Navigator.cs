using System;
using WpfApp2.ViewModels;

namespace WpfApp2.State;

/// <summary> Типы VM для главного окна - MainWindow </summary>
public enum ViewModelType
{
    /// <summary> главная VM </summary>
    Main,

    /// <summary> VM с группами и студентами </summary>
    GroupsStudents,

    /// <summary> VM с оценками </summary>
    AcademyJournal,

    /// <summary> VM с посещением </summary>
    VisitJournal,

    /// <summary> VM со всеми предметами </summary>
    AllSubjects,

    /// <summary> VM с предметами для группы </summary>
    SubjectsForGroup,

    /// <summary> VM для другого окна </summary>
    Other
}

public class Navigator
{
    public event Action StateChanged;

    private readonly ViewModelFactory _viewModelFactory;
    private BaseViewModel _currentViewModel;

    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            if(!Equals(_currentViewModel, value))
            {
                _currentViewModel?.Dispose();   
                _currentViewModel = value;
                StateChanged?.Invoke();
            }
        }
    }

    public Navigator(ViewModelFactory viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void ChangeState(ViewModelType viewModelType)
    {
        CurrentViewModel = _viewModelFactory.CreateViewModel(viewModelType);
    }
}
