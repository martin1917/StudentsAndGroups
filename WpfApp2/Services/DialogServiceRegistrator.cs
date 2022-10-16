using Microsoft.Extensions.DependencyInjection;
using WpfApp2.ViewModels.Utils;
using WpfApp2.ViewModels;

namespace WpfApp2.Services;

public static class DialogServiceRegistrator
{
    public static IServiceCollection AddDialogService(this IServiceCollection services) => services
        .AddTransient<CommonDialogService>()
        .AddTransient<StudentDialogService>()
        .AddTransient<GroupDialogService>()
        .AddTransient<SubjectDialogService>();
}
