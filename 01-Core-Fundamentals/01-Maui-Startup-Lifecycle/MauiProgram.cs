using Microsoft.Extensions.DependencyInjection;
using MauiMastery.StartupLifecycle.Services;
using MauiMastery.StartupLifecycle.ViewModels;

namespace MauiMastery.StartupLifecycle;

public static class MauiProgram
{
    public static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();
        // Реєстрація сервісів у DI-контейнері
        services.AddSingleton<IGreetingService, GreetingService>();
        services.AddTransient<StartupViewModel>();
        return services;
    }
}