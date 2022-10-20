using Microsoft.Extensions.Hosting;
using System;
using WpfApp2.Views.Windows;

namespace WpfApp2;

/// <summary> Главный класс </summary>
public class Program
{
    /// <summary>
    /// Метод запускающий приложение
    /// </summary>
    /// <param name="args">аргументы командной строки </param>
    [STAThread]
    public static void Main(string[] args)
    {
        var a = new HelloDialog();
        a.ShowDialog();

        var app = new App();
        app.InitializeComponent();
        app.Run();
    }

    /// <summary>
    /// Создание хост билдера
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureServices(App.ConfigureServices);
}
