using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp2.State;

namespace WpfApp2.ViewModels.Base;

/// <summary> Базовая VM </summary>
public abstract class BaseViewModel : INotifyPropertyChanged
{
    /// <summary> Событие, возникающей, когда менятеся значения свойств </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary> Тип VM </summary>
    public ViewModelType ViewModelType { get; init; }

    public BaseViewModel(ViewModelType viewModelType = ViewModelType.Other)
    {
        ViewModelType = viewModelType;
    }

    protected virtual bool Set<T>(ref T field, T val, [CallerMemberName] string prop = "")
    {
        if (Equals(field, val)) return false;
        field = val;
        OnPropertyChanged(prop);
        return true;
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    protected virtual void OnPropertyChanged(params string[] props)
    {
        foreach (string prop in props)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public virtual void Dispose() { }
}

