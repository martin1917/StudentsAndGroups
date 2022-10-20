using Microsoft.Extensions.Hosting;
using System;
using WpfApp2.Views.Windows;

namespace WpfApp2;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var a = new HelloDialog();
        a.ShowDialog();

        var app = new App();
        app.InitializeComponent();
        app.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureServices(App.ConfigureServices);
}
