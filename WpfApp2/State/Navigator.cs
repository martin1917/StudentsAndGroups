﻿using System;
using WpfApp2.ViewModels.Base;
using WpfApp2.ViewModels.Utils;

namespace WpfApp2.State;

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
