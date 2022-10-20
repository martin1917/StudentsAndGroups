using Microsoft.Extensions.DependencyInjection;

namespace WpfApp2.ViewModels.Utils;

/// <summary> Класс, который возвращает VM из DI контейнера </summary>
public class ViewModelLocator
{
    public MainViewModel MainWindowModel => App.Host.Services.GetRequiredService<MainViewModel>();
}
