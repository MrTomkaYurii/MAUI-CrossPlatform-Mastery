namespace TodoApp;

/// <summary>
/// Кореневий клас застосунку MAUI.
/// App.xaml.cs разом з App.xaml утворюють кореневий вузол застосунку.
/// Application — базовий клас MAUI для всіх застосунків.
/// </summary>
public partial class App : Application
{
    /// <summary>Shell зберігаємо, щоб передати у вікно через CreateWindow</summary>
    private readonly AppShell _shell;

    /// <summary>
    /// Конструктор: AppShell передається через DI, налаштовану в MauiProgram.cs.
    /// partial class — частина коду знаходиться у згенерованому App.xaml.g.cs.
    /// </summary>
    public App(AppShell shell)
    {
        // InitializeComponent — ініціалізує XAML-частину (ресурси з App.xaml)
        InitializeComponent();
        _shell = shell;
    }

    /// <summary>
    /// CreateWindow — новий спосіб встановлення кореневої сторінки (замість MainPage).
    /// MAUI 10 викликає цей метод замість встановлення MainPage безпосередньо.
    /// </summary>
    protected override Window CreateWindow(IActivationState? activationState)
        => new Window(_shell);
}
