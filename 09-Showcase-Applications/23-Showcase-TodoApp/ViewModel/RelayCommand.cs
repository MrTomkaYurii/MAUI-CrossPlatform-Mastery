using System.Windows.Input;

// Простір імен для ViewModel-компонентів
namespace TodoApp.ViewModel;

/// <summary>
/// Власна реалізація ICommand без сторонніх бібліотек.
///
/// RelayCommand дозволяє "загорнути" будь-який метод у об'єкт команди,
/// щоб XAML міг прив'язати кнопку до цього методу через Binding.
///
/// Замість того, щоб писати окремий клас для кожної команди,
/// ми просто передаємо Action (делегат на метод) у конструктор.
///
/// Приклад без RelayCommand (довго):
///   public class SaveCommand : ICommand { ... }
///
/// Приклад з RelayCommand (коротко):
///   SaveCommand = new RelayCommand(Save, () => !string.IsNullOrEmpty(Title));
/// </summary>
public class RelayCommand : ICommand
{
    // === Приватні поля ===

    /// <summary>
    /// Делегат — посилання на метод, який виконається при натисканні кнопки.
    /// Action — це тип делегата, що представляє метод без параметрів і без повернення значення.
    /// </summary>
    private readonly Action _execute;

    /// <summary>
    /// Опціональний делегат для перевірки, чи можна виконати команду.
    /// Func<bool>; — це тип делегата, що повертає bool.
    /// null означає "команда завжди доступна".
    /// </summary>
    private readonly Func<bool>? _canExecute;

    // === Подія ICommand ===

    /// <summary>
    /// MAUI підписується на цю подію, щоб знати, коли перевіряти CanExecute.
    /// Якщо CanExecute повертає false — кнопка автоматично стає сірою (IsEnabled = false).
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    // === Конструктор ===

    /// <summary>
    /// Створює нову команду.
    /// </summary>
    /// <param name="execute">Метод, який виконається при натисканні (обов'язковий)</param>
    /// <param name="canExecute">Умова доступності команди (необов'язковий)</param>
    /// <exception cref="ArgumentNullException">Якщо execute є null</exception>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        // Перевіряємо, що метод виконання передано — без нього команда не має сенсу
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    // === Методи ICommand ===

    /// <summary>
    /// Перевіряє, чи доступна команда зараз.
    /// Викликається MAUI автоматично, щоб увімкнути/вимкнути кнопку.
    ///
    /// parameter — зазвичай null для команд без параметрів.
    /// </summary>
    public bool CanExecute(object? parameter)
    {
        // Якщо _canExecute не задано (null) — команда завжди доступна (повертаємо true)
        // Якщо задано — викликаємо делегат і повертаємо його результат
        return _canExecute?.Invoke() ?? true;
    }

    /// <summary>
    /// Виконує команду — викликає збережений метод.
    /// Викликається MAUI, коли користувач натискає кнопку.
    /// </summary>
    public void Execute(object? parameter)
    {
        _execute(); // просто виконуємо збережений Action
    }

    /// <summary>
    /// Примусово сповіщаємо MAUI: "перевір CanExecute знову!".
    /// Виклич цей метод, коли умова доступності команди могла змінитись.
    ///
    /// Наприклад: після введення тексту в поле — перевіряємо, чи кнопка "Зберегти" тепер активна.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        // Invoke — надсилаємо подію всім підписникам (зазвичай це UI)
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

/// <summary>
/// Версія RelayCommand, яка приймає параметр типу T.
///
/// Використовується, коли команді потрібно знати, НА ЯКОМУ елементі вона спрацювала.
/// Наприклад: видалення конкретної справи зі списку (параметр = TodoItem).
///
/// У XAML параметр передається через CommandParameter:
///   Command="{Binding DeleteCommand}" CommandParameter="{Binding .}"
///   де {Binding .} означає "поточний елемент списку"
/// </summary>
public class RelayCommand<T> : ICommand
{
    /// <summary>Метод виконання, що приймає параметр типу T</summary>
    private readonly Action<T?> _execute;

    /// <summary>Умова доступності, що приймає параметр типу T</summary>
    private readonly Func<T?, bool>? _canExecute;

    /// <summary>Подія для оновлення стану кнопки</summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Створює нову команду з параметром.
    /// </summary>
    /// <param name="execute">Метод з параметром типу T</param>
    /// <param name="canExecute">Умова доступності з параметром типу T (необов'язково)</param>
    public RelayCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// MAUI передає параметр як object — ми кастимо його до типу T.
    /// (T?) — додаємо ? щоб дозволити null (на випадок, якщо XAML не передав параметр).
    /// </summary>
    public bool CanExecute(object? parameter) => _canExecute?.Invoke((T?)parameter) ?? true;

    /// <summary>Виконує команду, кастуючи параметр до типу T</summary>
    public void Execute(object? parameter) => _execute((T?)parameter);

    /// <summary>Оновлює стан кнопки — виклич після зміни умови CanExecute</summary>
    public void RaiseCanExecuteChanged() =>
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
