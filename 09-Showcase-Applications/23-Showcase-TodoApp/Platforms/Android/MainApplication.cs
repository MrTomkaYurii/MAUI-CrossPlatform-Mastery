using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace TodoApp;

/// <summary>
/// Клас Application для Android.
///
/// Application — глобальний об'єкт, що живе весь час роботи застосунку.
/// Тут ми говоримо Android, який MauiApp використовувати.
///
/// [Application] — реєструє цей клас як Application в AndroidManifest.
/// </summary>
[Application]
public class MainApplication : MauiApplication
{
    /// <summary>
    /// Конструктор, який Android викликає при створенні застосунку.
    /// handle та jniHandleOwnership — технічні параметри Android JNI (Java Native Interface).
    /// </summary>
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    /// <summary>
    /// Повертає налаштований MAUI застосунок.
    /// MAUI викликає цей метод для ініціалізації DI, сервісів, шрифтів.
    /// </summary>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
