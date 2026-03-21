using Android.App;
using Android.Content.PM;
using Microsoft.Maui;

namespace TodoApp;

/// <summary>
/// Головна Activity Android-застосунку.
///
/// Activity — основний будівельний блок Android. Кожен екран — це Activity.
/// MAUI використовує одну Activity (SingleTask), а навігацію між сторінками
/// виконує всередині неї через власний механізм (Shell.GoToAsync).
///
/// Атрибут [Activity] реєструє цей клас в AndroidManifest:
/// - MainLauncher = true: ця Activity запускається при старті застосунку
/// - Theme: тема для Splash Screen (ховається після завантаження)
/// - ConfigurationChanges: перелічуємо зміни конфігурації, які обробляємо самі
///   (без цього Android перезапускав би Activity при повороті екрана тощо)
/// </summary>
[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTask,
    ConfigurationChanges =
        ConfigChanges.ScreenSize |
        ConfigChanges.Orientation |
        ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize |
        ConfigChanges.Density
)]
public class MainActivity : MauiAppCompatActivity
{
    // MauiAppCompatActivity — базовий клас від MAUI, який поєднує
    // AppCompatActivity (Android) з MAUI-рушієм.
    // Весь необхідний код вже в базовому класі — нам нічого додавати.
}
