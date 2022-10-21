using System;
using System.Windows.Input;

namespace WpfApp2.Infrastructure.Commands;

/// <summary> Команда, устанавливающая резульат диалоговому окну </summary>
public class DialogResultCommand : ICommand
{
    /// <summary> Событие возникает, когда возможность выполнения команды изменяется </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary> Результат диалога </summary>
    public bool? DialogResult { get; set; }

    /// <summary> Проверить может ли быть выполнена команда </summary>
    /// <returns> true - если выполнение возможно; else - невозможно </returns>
    public bool CanExecute(object? parameter) => App.CurrentWindow != null;

    /// <summary> Выполнить команду </summary>
    public void Execute(object? parameter)
    {
        if (!CanExecute(parameter)) return;

        var window = App.CurrentWindow;

        var dialog_result = DialogResult;
        if (parameter != null)
            dialog_result = Convert.ToBoolean(parameter);

        window.DialogResult = dialog_result;
        window.Close();
    }
}
