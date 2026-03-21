using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoApp.ViewModel;

/// <summary>
/// Базовий клас для всіх ViewModel у проєкті.
///
/// Реалізує INotifyPropertyChanged, щоб кожен ViewModel, який успадковує BaseViewModel,
/// автоматично мав можливість сповіщати UI про зміни властивостей.
///
/// Навіщо базовий клас:
/// - Пишемо OnPropertyChanged і SetProperty один раз, а не в кожному ViewModel
/// - Всі ViewModel мають однакову поведінку сповіщень
/// - Якщо треба щось змінити в логіці сповіщень — міняємо в одному місці
///
/// Використання: MainViewModel : BaseViewModel (успадковуємо)
/// </summary>
public abstract class BaseViewModel : INotifyPropertyChanged
{
    // === INotifyPropertyChanged ===

    /// <summary>
    /// Подія, на яку підписується MAUI-рушій Binding.
    /// Кожен раз, коли властивість змінюється і ми викликаємо OnPropertyChanged,
    /// MAUI отримує цю подію і оновлює відповідний UI-елемент.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    // === Захищені методи (доступні у підкласах) ===

    /// <summary>
    /// Надсилає сповіщення про зміну властивості.
    ///
    /// [CallerMemberName] — магічний атрибут: якщо ти не передаєш propertyName,
    /// компілятор автоматично підставляє ім'я методу/властивості, звідки викликано цей метод.
    ///
    /// Тобто в сеттері властивості Name можна писати просто OnPropertyChanged(),
    /// і C# сам знає, що змінилась властивість "Name".
    /// </summary>
    /// <param name="propertyName">Ім'я властивості (заповнюється автоматично)</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        // ?.Invoke — безпечний виклик: якщо немає підписників, нічого не відбудеться
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Зручний метод для зміни значення поля з автоматичним сповіщенням.
    ///
    /// Зразок використання (у ViewModel):
    ///   private string _title;
    ///   public string Title
    ///   {
    ///       get => _title;
    ///       set => SetProperty(ref _title, value); // замість 4 рядків коду — 1
    ///   }
    ///
    /// SetProperty сам:
    /// 1. Перевіряє, чи справді значення змінилось (щоб не спамити зайвими подіями)
    /// 2. Записує нове значення у приватне поле
    /// 3. Викликає OnPropertyChanged для оновлення UI
    /// </summary>
    /// <typeparam name="T">Тип властивості (рядок, число, bool тощо)</typeparam>
    /// <param name="field">Посилання (ref) на приватне поле — щоб можна було змінити його</param>
    /// <param name="value">Нове значення</param>
    /// <param name="propertyName">Ім'я властивості (заповнюється автоматично)</param>
    /// <returns>true якщо значення змінилось, false якщо залишилось тим самим</returns>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        // Якщо нове значення = старому — нічого не робимо і повертаємо false
        // EqualityComparer<T>.Default.Equals — правильне порівняння для будь-якого типу
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;               // записуємо нове значення
        OnPropertyChanged(propertyName); // сповіщаємо UI
        return true;                 // повертаємо true: значення змінилось
    }
}
