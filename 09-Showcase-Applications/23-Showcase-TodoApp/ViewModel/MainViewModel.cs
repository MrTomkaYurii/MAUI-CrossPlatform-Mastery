using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoApp.Model;
using TodoApp.Repository;

namespace TodoApp.ViewModel;

/// <summary>
/// ViewModel для головної сторінки — список активних (невиконаних) справ.
///
/// Відповідає за:
/// - Завантаження активних справ з БД
/// - Позначення справи як виконаної (переміщення до архіву)
/// - Видалення справи
/// - Навігацію на сторінки "Додати" та "Архів"
///
/// Успадковує BaseViewModel, тому вже має INotifyPropertyChanged.
/// </summary>
public class MainViewModel : BaseViewModel
{
    // === Приватне поле для репозиторію ===

    /// <summary>
    /// Репозиторій для роботи з БД.
    /// Ключовий момент: ViewModel знає тільки про ITodoRepository (інтерфейс),
    /// а не про конкретну реалізацію SQLite. Це розв'язка залежностей.
    /// </summary>
    private readonly ITodoRepository _repository;

    // === Приватні поля для властивостей ===

    /// <summary>Чи виконується оновлення списку зараз?</summary>
    private bool _isRefreshing;

    // === Публічні властивості ===

    /// <summary>
    /// Колекція активних справ для відображення у CollectionView.
    ///
    /// ObservableCollection — особлива колекція, яка автоматично сповіщає UI
    /// про додавання, видалення та зміну елементів.
    /// Звичайний Lis<T>; не оновлює UI автоматично!
    ///
    /// CollectionView у XAML прив'язується до цієї колекції:
    ///   ItemsSource="{Binding Tasks}"
    /// </summary>
    public ObservableCollection<TodoItem> Tasks { get; } = new();

    /// <summary>
    /// Чи виконується pull-to-refresh прямо зараз?
    /// Прив'язується до RefreshView.IsRefreshing у XAML.
    /// </summary>
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value); // SetProperty з BaseViewModel
    }

    // === Команди ===

    /// <summary>
    /// Команда завантаження/оновлення списку справ.
    /// Викликається при pull-to-refresh та при появі сторінки.
    /// </summary>
    public RelayCommand RefreshCommand { get; }

    /// <summary>
    /// Команда позначення справи як виконаної.
    /// Приймає TodoItem як параметр (яку справу виконати).
    /// Після виконання справа зникає зі списку активних (переходить до архіву).
    /// </summary>
    public RelayCommand<TodoItem> CompleteTaskCommand { get; }

    /// <summary>
    /// Команда видалення справи назавжди.
    /// Приймає TodoItem як параметр (яку справу видалити).
    /// </summary>
    public RelayCommand<TodoItem> DeleteTaskCommand { get; }

    /// <summary>
    /// Команда навігації на сторінку "Додати нову справу".
    /// </summary>
    public RelayCommand NavigateToAddCommand { get; }

    /// <summary>
    /// Команда навігації на сторінку "Архів".
    /// </summary>
    public RelayCommand NavigateToArchiveCommand { get; }

    // === Конструктор ===

    /// <summary>
    /// Конструктор: отримує репозиторій через Dependency Injection.
    ///
    /// Dependency Injection (DI) — патерн, де залежності (ITodoRepository) передаються
    /// ззовні, а не створюються всередині класу. Це дозволяє легко замінити реалізацію.
    ///
    /// MAUI DI контейнер автоматично передасть ITodoRepository при створенні MainViewModel.
    /// </summary>
    /// <param name="repository">Репозиторій для роботи з даними</param>
    public MainViewModel(ITodoRepository repository)
    {
        // Зберігаємо посилання на репозиторій
        _repository = repository;

        // Ініціалізуємо всі команди в конструкторі
        // async void — єдиний спосіб зробити async команду в RelayCommand без бібліотек
        RefreshCommand = new RelayCommand(async () => await LoadTasksAsync());

        CompleteTaskCommand = new RelayCommand<TodoItem>(
            async item => await CompleteTaskAsync(item)
        );

        DeleteTaskCommand = new RelayCommand<TodoItem>(
            async item => await DeleteTaskAsync(item)
        );

        NavigateToAddCommand = new RelayCommand(
            async () => await Shell.Current.GoToAsync("add")
        );

        NavigateToArchiveCommand = new RelayCommand(
            async () => await Shell.Current.GoToAsync("archive")
        );
    }

    // === Публічні методи ===

    /// <summary>
    /// Завантажує активні справи з БД і оновлює колекцію Tasks.
    /// Викликається при появі сторінки (з code-behind) та при pull-to-refresh.
    ///
    /// async Task — метод є асинхронним (не блокує UI під час запиту до БД).
    /// await — чекаємо на результат, але UI залишається "живим".
    /// </summary>
    public async Task LoadTasksAsync()
    {
        IsRefreshing = true; // показуємо індикатор оновлення

        try
        {
            // Отримуємо активні справи з БД через репозиторій
            var tasks = await _repository.GetActiveTasksAsync();

            // Очищуємо поточний список і додаємо нові елементи
            // Кожен Clear() і Add() сповіщає CollectionView в UI
            Tasks.Clear();
            foreach (var task in tasks)
            {
                Tasks.Add(task);
            }
        }
        finally
        {
            // finally гарантує, що IsRefreshing = false навіть при помилці
            IsRefreshing = false;
        }
    }

    // === Приватні методи ===

    /// <summary>
    /// Позначає справу як виконану і переміщує її до архіву.
    ///
    /// Кроки:
    /// 1. Встановлюємо IsCompleted = true і час виконання
    /// 2. Оновлюємо запис у БД
    /// 3. Видаляємо з колекції Tasks (UI автоматично видалить картку зі списку)
    /// </summary>
    /// <param name="item">Справа для виконання</param>
    private async Task CompleteTaskAsync(TodoItem? item)
    {
        // Перевіряємо, що item не є null (захист від помилок)
        if (item is null) return;

        // Позначаємо справу як виконану
        item.IsCompleted = true;

        // Фіксуємо точний час виконання
        item.CompletedAt = DateTime.Now;

        // Зберігаємо зміни в БД (UPDATE)
        await _repository.UpdateTaskAsync(item);

        // Видаляємо з колекції — UI автоматично прибере картку
        // ObservableCollection надсилає сповіщення CollectionView про видалення
        Tasks.Remove(item);
    }

    /// <summary>
    /// Видаляє справу назавжди (з БД і з UI).
    ///
    /// Кроки:
    /// 1. Видаляємо запис з БД (DELETE)
    /// 2. Видаляємо з колекції Tasks
    /// </summary>
    /// <param name="item">Справа для видалення</param>
    private async Task DeleteTaskAsync(TodoItem? item)
    {
        if (item is null) return;

        // Видаляємо з бази даних назавжди
        await _repository.DeleteTaskAsync(item);

        // Видаляємо з ObservableCollection — UI оновиться автоматично
        Tasks.Remove(item);
    }
}
