using Microsoft.Extensions.DependencyInjection;

namespace WpfApp2.Managers;

public static class ManagerRegistrator
{
    public static IServiceCollection AddManagers(this IServiceCollection services) => services
        .AddTransient<StudentManager>()
        .AddTransient<GroupManager>()
        .AddTransient<SubjectManager>();
}
