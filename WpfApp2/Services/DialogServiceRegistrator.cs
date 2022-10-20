using Microsoft.Extensions.DependencyInjection;

namespace WpfApp2.Services;

/// <summary> Регистрация всех диалоговых сервисов в DI контейнере </summary>
public static class DialogServiceRegistrator
{
    public static IServiceCollection AddDialogService(this IServiceCollection services) => services
        .AddTransient<CommonDialogService>()
        .AddTransient<StudentDialogService>()
        .AddTransient<GroupDialogService>()
        .AddTransient<SubjectDialogService>();
}
