using System.Windows.Input;
using TodoApp.Model;
using TodoApp.Repository;

namespace TodoApp.ViewModel;

/// <summary>
/// ViewModel для сторінки додавання нової справи.
///
/// Відповідає за:
/// - Зберігання введених назви та опису
/// - Валідацію: кнопка "Зберегти" активна тільки якщо назва не порожня
/// - Збереження нової справи в БД
/// - Навігацію назад на головну сторінку після збереження
/// </summary>
public class AddTaskViewModel : BaseViewModel
{
    // === Приватне поле для репозиторію ===

    /// <summary>Репозиторій для збереження нових справ у БД</summary>
    private readonly ITodoRepository _repository;

    // === Приватні поля для властивостей ===

    /// <summary>Введена назва справи</summary>
    private string _title = string.Empty;

    /// <summary>Введений опис справи</summary>
    private string _description = string.Empty;

    // === Публічні властивості ===

    /// <summary>
    /// Назва нової справи.
    /// Прив'язана до Entry у XAML через двостороннє Binding (TwoWay).
    ///
    /// При кожній зміні назви ми також викликаємо RaiseCanExecuteChanged,
    /// щоб MAUI перевірив, чи стала кнопка "Зберегти" активною.
    /// </summary>
    public string Title
    {
        get => _title;
        set
        {
            // SetProperty: записує значення і сповіщає UI (з BaseViewModel)
            SetProperty(ref _title, value);

            // Після зміни назви перевіряємо, чи можна зберегти
            // (кнопка стає активною, коли назва не порожня)
            SaveCommand.RaiseCanExecuteChanged();
        }
    }

    /// <summary>
    /// Опис нової справи (необов'язковий).
    /// Прив'язаний до Editor у XAML через двостороннє Binding.
    /// </summary>
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    // === Команди ===

    /// <summary>
    /// Команда збереження нової справи.
    ///
    /// CanExecute: активна тільки якщо Title не порожній.
    /// MAUI автоматично вмикає/вимикає кнопку залежно від CanExecute.
    ///
    /// Тип: RelayCommand (не generic), бо параметр не потрібен.
    /// </summary>
    public RelayCommand SaveCommand { get; }

    /// <summary>
    /// Команда скасування — повертає на головну сторінку без збереження.
    /// </summary>
    public RelayCommand CancelCommand { get; }

    // === Конструктор ===

    /// <summary>
    /// Ініціалізує ViewModel і всі команди.
    /// Репозиторій передається через DI.
    /// </summary>
    public AddTaskViewModel(ITodoRepository repository)
    {
        _repository = repository;

        // SaveCommand — активний тільки якщо назва не порожня
        // canExecute: перевіряємо при кожному виклику CanExecute
        SaveCommand = new RelayCommand(
            execute: async () => await SaveTaskAsync(),
            canExecute: () => !string.IsNullOrWhiteSpace(_title)
        );

        // CancelCommand — повертаємо на попередню сторінку
        // Shell.Current.GoToAsync("..") — еквівалент "назад" у навігації
        CancelCommand = new RelayCommand(
            async () => await Shell.Current.GoToAsync("..")
        );
    }

    // === Публічні методи ===

    /// <summary>
    /// Скидає форму до початкового стану.
    /// Викликається при появі сторінки, щоб поля були порожніми.
    /// </summary>
    public void ResetForm()
    {
        Title = string.Empty;
        Description = string.Empty;
    }

    // === Приватні методи ===

    /// <summary>
    /// Зберігає нову справу в базі даних і повертає на головну сторінку.
    ///
    /// Кроки:
    /// 1. Створюємо об'єкт TodoItem з введеними даними
    /// 2. Зберігаємо в БД через репозиторій
    /// 3. Переходимо назад на головну сторінку
    /// 4. Скидаємо форму (щоб при наступному відкритті поля були порожніми)
    /// </summary>
    private async Task SaveTaskAsync()
    {
        // Створюємо новий об'єкт моделі з введеними даними
        var newTask = new TodoItem
        {
            Title = _title.Trim(),          // Trim() прибирає зайві пробіли на початку/кінці
            Description = _description.Trim(),
            IsCompleted = false,            // нова справа завжди активна
            CreatedAt = DateTime.Now        // фіксуємо час створення
        };

        // Зберігаємо в БД — після цього newTask.Id буде заповнено автоматично SQLite
        await _repository.AddTaskAsync(newTask);

        // Повертаємось на головну сторінку (Shell навігація)
        // ".." означає "на один рівень назад у навігаційному стеку"
        await Shell.Current.GoToAsync("..");

        // Скидаємо форму для наступного використання
        ResetForm();
    }
}
