using Microsoft.Extensions.DependencyInjection;
using WpfApp2.Services;

namespace WpfApp2.ViewModels.Utils;

/// <summary> Регистрация VM и фабрик по их созданию в DI контейнере </summary>
public static class ViewModelRegistrator
{
    public static IServiceCollection AddViewModels(this IServiceCollection services) => services
        .AddTransient<GroupsStudentsViewModel>()
        .AddTransient<AllSubjectsViewModel>()
        .AddTransient<SubjectForGroupViewModel>()
        .AddTransient<JournalViewModel>()
        .AddTransient<AvgMarksViewModel>()
        .AddTransient<MainViewModel>();

    public static IServiceCollection AddFactoryViewModels(this IServiceCollection services) => services
        .AddSingleton<CreateViewModel<GroupsStudentsViewModel>>(s => () => s.GetRequiredService<GroupsStudentsViewModel>())
        .AddSingleton<CreateViewModel<AllSubjectsViewModel>>(s => () => s.GetRequiredService<AllSubjectsViewModel>())
        .AddSingleton<CreateViewModel<SubjectForGroupViewModel>>(s => () => s.GetRequiredService<SubjectForGroupViewModel>())
        .AddSingleton<CreateViewModel<JournalViewModel>>(s => () => s.GetRequiredService<JournalViewModel>())
        .AddSingleton<CreateViewModel<AvgMarksViewModel>>(s => () => s.GetRequiredService<AvgMarksViewModel>())
        .AddSingleton<ViewModelFactory>();
}
