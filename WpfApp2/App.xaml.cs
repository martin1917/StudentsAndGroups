﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using WpfApp2.Data;
using WpfApp2.State;
using WpfApp2.ViewModels;

namespace WpfApp2;

public partial class App
{
    private static IHost? __host;
    public static IHost Host => __host
        ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).ConfigureAppConfiguration(conf =>
        {
            conf.AddJsonFile("appsettings.json");
            conf.AddEnvironmentVariables();
        }).Build();

    public static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
        .AddDbContext<Context>(opt =>
        {
            IConfiguration configuration = host.Configuration.GetSection("DataBases");
            var type = configuration["Type"];

            switch (type)
            {
                case null: throw new InvalidOperationException("Не определён тип БД");
                default: throw new InvalidOperationException($"Тип подключения {type} не поддерживается");

                case "SQLite":
                    opt.UseSqlite(configuration.GetConnectionString(type));
                    break;
            }

        })
        .AddTransient<DbInitializer>()
        .AddTransient<GroupsStudentsViewModel>()
        .AddTransient<MainViewModel>()
        .AddSingleton<CreateViewModel<GroupsStudentsViewModel>>(s => () => s.GetRequiredService<GroupsStudentsViewModel>())
        .AddSingleton<ViewModelFactory>()
        .AddSingleton<Navigator>();

    protected override void OnStartup(StartupEventArgs e)
    {
        var host = Host;

        //using (var scope = host.Services.CreateScope())
        //{
        //    scope.ServiceProvider.GetRequiredService<DbInitializer>().Initialize();
        //}

        base.OnStartup(e);
        host.Start();
    }
}
