using Microsoft.Extensions.DependencyInjection;

namespace WpfApp2.ViewModels;

public class ViewModelLocator
{
    public MainViewModel MainWindowModel => App.Host.Services.GetRequiredService<MainViewModel>();
}
