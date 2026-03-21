using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace TodoApp.WinUI;

/// <summary>
/// Точка входу WinUI застосунку для платформи Windows.
///
/// MauiWinUIApplication — базовий клас від MAUI, який:
/// - Ініціалізує WinUI (нативний UI-рушій Windows)
/// - Викликає CreateMauiApp() щоб отримати налаштований MAUI застосунок
/// - Зв'язує WinUI вікно з MAUI Shell і сторінками
///
/// Цей файл компілюється ТІЛЬКИ для Windows (знаходиться в Platforms/Windows/).
/// </summary>
public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Конструктор: ініціалізує WinUI-частину застосунку.
    /// InitializeComponent() обробляє App.xaml.
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Повертає налаштований MAUI застосунок.
    /// MAUI викликає цей метод для отримання точки входу бізнес-логіки.
    /// MauiProgram.CreateMauiApp() налаштовує DI, шрифти, сервіси.
    /// </summary>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
