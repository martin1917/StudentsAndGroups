using System;
using System.Windows.Input;

namespace WpfApp2.Infrastructure.Commands;

/// <summary> Класс команды </summary>
public class Command : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    /// <summary> Событие возникает, когда возможность выполнения команды изменяется </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary> Конструктор </summary>
    /// <param name="execute">Делегат, который будет выполнен, при выполнении команды</param>
    /// <param name="canExecute">Делегат, который будет выполнен, при проверке может ли быть выполнена команда </param>
    public Command(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    /// <summary> Проверить может ли быть выполнена команда </summary>
    /// <returns> true - если выполнение возможно; else - невозможно </returns>
    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

    /// <summary> Выполнить команду </summary>
    public void Execute(object? parameter)
    {
        if (CanExecute(parameter))
        {
            _execute(parameter);
        }
    }
}
