using System.Windows.Input;

namespace WpfApp2.Views.Windows;

public partial class HelloDialog
{
    public HelloDialog()
        => InitializeComponent();
    
    private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        => Close();
}
