using Microsoft.Extensions.DependencyInjection;

namespace WpfApp2.Managers;

/// <summary> Класс регистрирующий все менеджеры в DI контейнере </summary>
public static class ManagerRegistrator
{
    public static IServiceCollection AddManagers(this IServiceCollection services) => services
        .AddTransient<StudentManager>()
        .AddTransient<GroupManager>()
        .AddTransient<SubjectManager>();
}
