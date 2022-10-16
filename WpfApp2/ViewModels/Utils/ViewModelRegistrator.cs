using Microsoft.Extensions.DependencyInjection;
using WpfApp2.Services;

namespace WpfApp2.ViewModels.Utils;

public static class ViewModelRegistrator
{
    public static IServiceCollection AddViewModels(this IServiceCollection services) => services
        .AddTransient<GroupsStudentsViewModel>()
        .AddTransient<AllSubjectsViewModel>()
        .AddTransient<SubjectForGroupViewModel>()
        .AddTransient<MainViewModel>();

    public static IServiceCollection AddFactoryViewModels(this IServiceCollection services) => services
        .AddSingleton<CreateViewModel<GroupsStudentsViewModel>>(s => () => s.GetRequiredService<GroupsStudentsViewModel>())
        .AddSingleton<CreateViewModel<AllSubjectsViewModel>>(s => () => s.GetRequiredService<AllSubjectsViewModel>())
        .AddSingleton<CreateViewModel<SubjectForGroupViewModel>>(s => () => s.GetRequiredService<SubjectForGroupViewModel>())
        .AddSingleton<ViewModelFactory>();
}
