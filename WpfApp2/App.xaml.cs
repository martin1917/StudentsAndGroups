using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using WpfApp2.Data;
using WpfApp2.Managers;
using WpfApp2.Mapper;
using WpfApp2.Services;
using WpfApp2.State;
using WpfApp2.ViewModels.Utils;
using WpfApp2.Views.Windows;

namespace WpfApp2;

public partial class App
{
    /// <summary> получение активного окна </summary>
    public static Window? ActiveWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

    /// <summary> получение окна, на котором сейчас фокус </summary>
    public static Window? FocusedWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsFocused);

    /// <summary> получение текущего окна </summary>
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
    /// <param name="host"></param>
    /// <param name="services"></param>
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
