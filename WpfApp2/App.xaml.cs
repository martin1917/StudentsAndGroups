using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Windows;
using WpfApp2.Data;
using WpfApp2.Mapper;
using WpfApp2.Services;
using WpfApp2.State;
using WpfApp2.ViewModels.Utils;

namespace WpfApp2;

public partial class App
{
    public static Window? ActiveWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

    public static Window? FocusedWindow => Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsFocused);

    public static Window? CurrentWindow => FocusedWindow ?? ActiveWindow;

    private static IHost? __host;
    public static IHost Host => __host
        ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).ConfigureAppConfiguration(conf =>
        {
            conf.AddJsonFile("appsettings.json");
            conf.AddEnvironmentVariables();
        }).Build();

    public static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
        .AddTransient<DbInitializer>()
        .AddAutoMapper(typeof(AppMappingProfile))
        .AddViewModels()
        .AddFactoryViewModels()
        .AddSingleton<Navigator>()
        .AddDialogService();

    protected override void OnStartup(StartupEventArgs e)
    {
        var host = Host;
        base.OnStartup(e);
        host.Start();
    }
}