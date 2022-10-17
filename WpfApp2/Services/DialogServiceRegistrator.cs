using Microsoft.Extensions.DependencyInjection;

namespace WpfApp2.Services;

public static class DialogServiceRegistrator
{
    public static IServiceCollection AddDialogService(this IServiceCollection services) => services
        .AddTransient<CommonDialogService>()
        .AddTransient<StudentDialogService>()
        .AddTransient<GroupDialogService>()
        .AddTransient<SubjectDialogService>();
}
