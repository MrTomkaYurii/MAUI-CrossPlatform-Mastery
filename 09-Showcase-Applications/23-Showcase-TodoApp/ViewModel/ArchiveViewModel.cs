using System.Collections.ObjectModel;
using TodoApp.Model;
using TodoApp.Repository;

namespace TodoApp.ViewModel;

/// <summary>
/// ViewModel для сторінки архіву — список виконаних справ.
///
/// Логіка схожа на MainViewModel, але:
/// - Завантажуємо виконані (IsCompleted = true) справи
/// - Можна тільки видалити справу (не можна "розвиконати")
/// - Є навігація на головну і сторінку додавання
/// </summary>
public class ArchiveViewModel : BaseViewModel
{
    // === Приватне поле репозиторію ===

    /// <summary>Репозиторій для доступу до архівованих справ</summary>
    private readonly ITodoRepository _repository;

    // === Приватні поля ===

    /// <summary>Чи виконується оновлення списку зараз?</summary>
    private bool _isRefreshing;

    // === Публічні властивості ===

    /// <summary>
    /// Колекція архівованих (виконаних) справ.
    /// ObservableCollection автоматично оновлює CollectionView при зміні.
    /// </summary>
    public ObservableCollection<TodoItem> ArchivedTasks { get; } = new();

    /// <summary>
    /// Чи виконується pull-to-refresh?
    /// Прив'язується до RefreshView.IsRefreshing у XAML.
    /// </summary>
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value); // з BaseViewModel
    }

    // === Команди ===

    /// <summary>
    /// Команда завантаження/оновлення архіву.
    /// Викликається при появі сторінки і при pull-to-refresh.
    /// </summary>
    public RelayCommand RefreshCommand { get; }

    /// <summary>
    /// Команда видалення справи з архіву назавжди.
    /// Приймає TodoItem — яку справу видалити.
    /// </summary>
    public RelayCommand<TodoItem> DeleteTaskCommand { get; }

    /// <summary>Команда навігації на головну сторінку</summary>
    public RelayCommand NavigateToMainCommand { get; }

    /// <summary>Команда навігації на сторінку додавання справи</summary>
    public RelayCommand NavigateToAddCommand { get; }

    // === Конструктор ===

    /// <summary>
    /// Ініціалізує ViewModel і всі команди.
    /// ITodoRepository передається через Dependency Injection.
    /// </summary>
    public ArchiveViewModel(ITodoRepository repository)
    {
        _repository = repository;

        // Ініціалізуємо всі команди
        RefreshCommand = new RelayCommand(async () => await LoadArchivedTasksAsync());

        DeleteTaskCommand = new RelayCommand<TodoItem>(
            async item => await DeleteTaskAsync(item)
        );

        // Shell.Current.GoToAsync("//main") — перехід до кореневої сторінки Shell
        // "//" означає "абсолютний шлях" (скидає навігаційний стек)
        NavigateToMainCommand = new RelayCommand(
            async () => await Shell.Current.GoToAsync("//main")
        );

        NavigateToAddCommand = new RelayCommand(
            async () => await Shell.Current.GoToAsync("add")
        );
    }

    // === Публічні методи ===

    /// <summary>
    /// Завантажує архівовані справи з БД і оновлює колекцію ArchivedTasks.
    /// Викликається з code-behind при появі сторінки.
    /// </summary>
    public async Task LoadArchivedTasksAsync()
    {
        IsRefreshing = true;

        try
        {
            // Отримуємо виконані справи з БД
            var tasks = await _repository.GetArchivedTasksAsync();

            // Оновлюємо колекцію
            ArchivedTasks.Clear();
            foreach (var task in tasks)
            {
                ArchivedTasks.Add(task);
            }
        }
        finally
        {
            // Завжди вимикаємо індикатор оновлення (навіть при помилці)
            IsRefreshing = false;
        }
    }

    // === Приватні методи ===

    /// <summary>
    /// Видаляє справу з архіву назавжди.
    ///
    /// Кроки:
    /// 1. Видаляємо з БД (DELETE)
    /// 2. Видаляємо з ObservableCollection (UI оновиться автоматично)
    /// </summary>
    /// <param name="item">Справа для видалення</param>
    private async Task DeleteTaskAsync(TodoItem? item)
    {
        if (item is null) return;

        // Видаляємо з БД
        await _repository.DeleteTaskAsync(item);

        // Видаляємо з колекції — CollectionView оновиться автоматично
        ArchivedTasks.Remove(item);
    }
}
