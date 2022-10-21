using System.Windows;

namespace WpfApp2.Services;

/// <summary> Класс, выводящий на экран различные собщения в MessageBox </summary>
public class CommonDialogService
{
    /// <summary>
    /// Подтверждение своих действий
    /// </summary>
    /// <param name="information">сообщение</param>
    /// <param name="caption">Заголовок</param>
    /// <returns></returns>
    public bool ConfirmInformation(string information, string caption)
    {
        return MessageBox.Show(information, caption, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes;
    }

    /// <summary>
    /// Вывод сообщения об ошибке валидации
    /// </summary>
    /// <param name="message">сообщение с ошибкой</param>
    public void ShowErrors(string message)
    {
        MessageBox.Show(message, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
