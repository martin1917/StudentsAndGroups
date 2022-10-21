using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Windows;
using WpfApp2.Data;
using WpfApp2.Managers;
using WpfApp2.Mapper;
using WpfApp2.Services;
using WpfApp2.State;
using WpfApp2.ViewModels.Utils;

namespace WpfApp2;

/// <summary> Класс приложения </summary>
public partial class App
{
    /// <summary> Получение активного окна </summary>
    public static Window? ActiveWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

    /// <summary> Получение окна, на котором сейчас фокус </summary>
    public static Window? FocusedWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsFocused);

    /// <summary> Получение текущего окна </summary>
    public static Window? CurrentWindow => FocusedWindow ?? ActiveWindow;

    private static IHost? __host;

    /// <summary> Хост </summary>
    public static IHost Host => __host
        ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).ConfigureAppConfiguration(conf =>
        {
            conf.AddJsonFile("appsettings.json");
            conf.AddEnvironmentVariables();
        }).Build();

    /// <summary>
    /// Конфигурация сервисов
    /// </summary>
    /// <param name="host">Хост</param>
    /// <param name="services">Контейнер с сервисами</param>
    public static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
        .AddTransient<DbInitializer>()
        .AddAutoMapper(typeof(AppMappingProfile))
        .AddViewModels()
        .AddFactoryViewModels()
        .AddSingleton<Navigator>()
        .AddDialogService()
        .AddManagers();

    protected override void OnStartup(StartupEventArgs e)
    {
        var host = Host;
        base.OnStartup(e);
        host.Start();
    }
}
