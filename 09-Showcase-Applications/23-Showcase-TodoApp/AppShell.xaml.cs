using TodoApp.View;

namespace TodoApp;

/// <summary>
/// Code-behind для AppShell.xaml — навігаційна оболонка застосунку.
///
/// Тут реєструємо маршрути для сторінок, до яких не можна перейти
/// напряму через ShellContent (вони не є кореневими сторінками).
///
/// Shell.Current.GoToAsync("add") — навігація до AddTaskPage.
/// Shell.Current.GoToAsync("archive") — навігація до ArchivePage.
/// </summary>
public partial class AppShell : Shell
{
    /// <summary>
    /// Конструктор: ініціалізує Shell та реєструє маршрути навігації.
    /// </summary>
    public AppShell()
    {
        // Ініціалізує XAML-частину (ShellContent з MainPage)
        InitializeComponent();

        // Реєструємо маршрут для сторінки додавання справи.
        // Після реєстрації можна навігуватись: await Shell.Current.GoToAsync("add")
        // ".." повертає назад: await Shell.Current.GoToAsync("..")
        Routing.RegisterRoute("add", typeof(AddTaskPage));

        // Реєструємо маршрут для сторінки архіву.
        // Повернення на головну: await Shell.Current.GoToAsync("//main")
        Routing.RegisterRoute("archive", typeof(ArchivePage));
    }
}
