using System;
using WpfApp2.ViewModels.Base;
using WpfApp2.ViewModels.Utils;

namespace WpfApp2.State;

/// <summary> Класс навигатор, который будет менять содержимое окна </summary>
public class Navigator
{
    /// <summary> Событие, возникающее, когда меняется текущая VM </summary>
    public event Action StateChanged;

    private readonly ViewModelFactory _viewModelFactory;
    private BaseViewModel _currentViewModel;

    /// <summary> Текущая VM </summary>
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

    /// <summary>
    /// Смена текущей VM
    /// </summary>
    /// <param name="viewModelType">Тип VM, на которую нужно сменить текущую</param>
    public void ChangeState(ViewModelType viewModelType)
    {
        CurrentViewModel = _viewModelFactory.CreateViewModel(viewModelType);
    }
}
