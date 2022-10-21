using System.Windows.Input;

namespace WpfApp2.Views.Windows;

/// <summary> Диалог с приветствием </summary>
public partial class HelloDialog
{
    /// <summary> Конструктор </summary>
    public HelloDialog()
        => InitializeComponent();
    
    private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        => Close();
}
