using TodoApp.Model;

// Простір імен для репозиторіїв (шар роботи з даними)
namespace TodoApp.Repository;

/// <summary>
/// Інтерфейс репозиторію справ.
///
/// Визначає КОНТРАКТ — набір операцій, які можна виконати зі справами.
/// ViewModel працює тільки з цим інтерфейсом, а не з конкретним класом.
///
/// Навіщо це потрібно:
/// - ViewModel не залежить від SQLite (можна замінити на REST API без змін у ViewModel)
/// - Легко написати тестову реалізацію (InMemoryRepository) для юніт-тестів
/// - Уся логіка БД знаходиться в одному місці
/// </summary>
public interface ITodoRepository
{
    /// <summary>
    /// Ініціалізація бази даних: створення таблиці, якщо вона ще не існує.
    /// Викликається один раз при старті застосунку.
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Отримати всі активні (невиконані) справи.
    /// Повертає список справ, де IsCompleted = false.
    /// </summary>
    Task<List<TodoItem>> GetActiveTasksAsync();

    /// <summary>
    /// Отримати всі архівовані (виконані) справи.
    /// Повертає список справ, де IsCompleted = true.
    /// Відсортовані за датою виконання (найновіші спочатку).
    /// </summary>
    Task<List<TodoItem>> GetArchivedTasksAsync();

    /// <summary>
    /// Додати нову справу до бази даних.
    /// Повертає кількість доданих рядків (зазвичай 1).
    /// </summary>
    /// <param name="item">Справа для збереження</param>
    Task<int> AddTaskAsync(TodoItem item);

    /// <summary>
    /// Оновити існуючу справу (наприклад, позначити як виконану).
    /// Пошук запису відбувається за Id.
    /// Повертає кількість оновлених рядків.
    /// </summary>
    /// <param name="item">Справа з оновленими значеннями</param>
    Task<int> UpdateTaskAsync(TodoItem item);

    /// <summary>
    /// Видалити справу з бази даних назавжди.
    /// Пошук запису відбувається за Id.
    /// Повертає кількість видалених рядків.
    /// </summary>
    /// <param name="item">Справа для видалення</param>
    Task<int> DeleteTaskAsync(TodoItem item);
}
