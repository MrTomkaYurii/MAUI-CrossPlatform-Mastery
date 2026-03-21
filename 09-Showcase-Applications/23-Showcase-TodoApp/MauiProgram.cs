using TodoApp.Repository;
using TodoApp.View;
using TodoApp.ViewModel;

namespace TodoApp;

/// <summary>
/// Точка входу застосунку MAUI.
///
/// MauiProgram — аналог Program.cs у ASP.NET Core.
/// Тут ми налаштовуємо Dependency Injection (DI) — систему,
/// яка автоматично створює об'єкти і передає їх у конструктори.
///
/// DI дозволяє:
/// - Не створювати об'єкти вручну (new MainViewModel(new TodoRepository()))
/// - Легко замінювати реалізації (наприклад, для тестів)
/// - Контролювати час життя об'єктів (Singleton, Transient тощо)
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Будує та повертає MauiApp — готовий до запуску застосунок.
    /// Цей метод викликається системою MAUI при старті.
    /// </summary>
    public static MauiApp CreateMauiApp()
    {
        // CreateBuilder — створює об'єкт-будівник застосунку
        var builder = MauiApp.CreateBuilder();

        builder
            // UseMauiApp&lt;App&gt; — встановлює App як кореневий клас застосунку
            .UseMauiApp<App>()
            // ConfigureFonts — реєстрація шрифтів для використання у XAML
            .ConfigureFonts(fonts =>
            {
                // OpenSans — стандартний шрифт MAUI (Regular і Semibold варіанти)
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // === Реєстрація сервісів у DI контейнері ===

        // --- Репозиторій ---
        // AddSingleton — один екземпляр на весь час роботи застосунку.
        // Singleton підходить для репозиторію: нам потрібне одне підключення до БД.
        // ITodoRepository — інтерфейс, TodoRepository — реалізація.
        // Коли хтось запитує ITodoRepository, DI поверне TodoRepository.
        builder.Services.AddSingleton<ITodoRepository, TodoRepository>();

        // --- ViewModels ---
        // AddTransient — новий екземпляр кожного разу.
        // Transient підходить для ViewModel: кожна сторінка має свій ViewModel.
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<AddTaskViewModel>();
        builder.Services.AddTransient<ArchiveViewModel>();

        // --- Сторінки (Views) ---
        // Сторінки також реєструємо, щоб DI міг передати ViewModel у їх конструктори.
        // AddTransient: нова сторінка при кожній навігації.
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AddTaskPage>();
        builder.Services.AddTransient<ArchivePage>();

        // AppShell реєструємо окремо — він потрібен для App і є singleton
        builder.Services.AddSingleton<AppShell>();


        // Будуємо застосунок
        var app = builder.Build();

        // === Ініціалізація БД ===
        // Отримуємо репозиторій з DI і ініціалізуємо БД (створюємо таблицю).
        // Це потрібно зробити до відкриття будь-якої сторінки.
        var repository = app.Services.GetRequiredService<ITodoRepository>();

        // Task.Run + .Wait() — запускаємо async метод синхронно (до старту UI).
        // Це безпечно тут, бо UI ще не запущено.
        Task.Run(async () => await repository.InitializeAsync()).Wait();

        return app;
    }
}
