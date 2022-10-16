using System;
using System.Windows;

namespace WpfApp2.Services;

public class CommonDialogService
{
    public bool ConfirmInformation(string information, string caption)
    {
        return MessageBox.Show(information, caption, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes;
    }

    public bool ConfirmWarning(string warning, string caption)
    {
        return MessageBox.Show(warning, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
    }

    public void ShowErrors(string message)
    {
        MessageBox.Show(message, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
