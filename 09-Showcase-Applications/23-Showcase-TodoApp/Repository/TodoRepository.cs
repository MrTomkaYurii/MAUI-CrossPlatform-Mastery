using SQLite;
using TodoApp.Model;

namespace TodoApp.Repository;

/// <summary>
/// Реалізація репозиторію справ через SQLite.
///
/// Цей клас знає, ЯК зберігати дані (у файлі SQLite).
/// ViewModel цього не знає — вона працює тільки через інтерфейс ITodoRepository.
///
/// SQLiteAsyncConnection — асинхронне підключення до БД.
/// Асинхронність важлива: операції з БД не блокують UI-потік
/// (користувач може взаємодіяти з застосунком, поки БД виконує запит).
/// </summary>
public class TodoRepository : ITodoRepository
{
    // === Приватні поля ===

    /// <summary>
    /// Асинхронне з'єднання з базою даних SQLite.
    /// Створюється в конструкторі і живе весь час роботи застосунку.
    /// </summary>
    private readonly SQLiteAsyncConnection _database;

    // === Конструктор ===

    /// <summary>
    /// Створює репозиторій і ініціалізує підключення до SQLite.
    ///
    /// FileSystem.AppDataDirectory — стандартний шлях для зберігання даних застосунку.
    /// На Android: /data/data/com.example.todoapp/files/
    /// На iOS: /var/mobile/.../Documents/
    /// На Windows: C:\Users\...\AppData\Local\TodoApp\
    ///
    /// Це правильне місце для БД — воно приватне і зберігається між запусками.
    /// </summary>
    public TodoRepository()
    {
        // Будуємо повний шлях до файлу бази даних
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "todo.db3");

        // Створюємо асинхронне підключення
        // SQLiteAsyncConnection не одразу відкриває файл — це відбувається при першому запиті
        _database = new SQLiteAsyncConnection(dbPath);
    }

    // === Методи інтерфейсу ITodoRepository ===

    /// <summary>
    /// Ініціалізація: створення таблиці TodoItems у БД, якщо вона ще не існує.
    ///
    /// CreateTableAsync — перевіряє наявність таблиці і створює її за атрибутами класу TodoItem.
    /// Якщо таблиця вже є — нічого не відбувається (безпечно викликати при кожному старті).
    ///
    /// Виклик: один раз при запуску застосунку в MauiProgram.cs
    /// </summary>
    public async Task InitializeAsync()
    {
        // CreateTableAsync читає атрибути класу TodoItem ([Table], [PrimaryKey] тощо)
        // і генерує SQL: CREATE TABLE IF NOT EXISTS TodoItems (Id INTEGER PRIMARY KEY AUTOINCREMENT, ...)
        await _database.CreateTableAsync<TodoItem>();
    }

    /// <summary>
    /// Отримати всі активні (невиконані) справи.
    ///
    /// Table&lt;TodoItem&gt;() — відкриває таблицю TodoItems для LINQ-запитів.
    /// Where — фільтр: тільки записи де IsCompleted = false.
    /// OrderByDescending — сортуємо від найновіших до найстаріших.
    /// ToListAsync — виконує запит і повертає список.
    /// </summary>
    public async Task<List<TodoItem>> GetActiveTasksAsync()
    {
        return await _database
            .Table<TodoItem>()                                 // SELECT * FROM TodoItems
            .Where(t => !t.IsCompleted)                        // WHERE IsCompleted = 0
            .OrderByDescending(t => t.CreatedAt)               // ORDER BY CreatedAt DESC
            .ToListAsync();                                    // виконати та повернути List
    }

    /// <summary>
    /// Отримати всі архівовані (виконані) справи.
    ///
    /// Логіка аналогічна GetActiveTasksAsync, але з протилежним фільтром (IsCompleted = true).
    /// Сортуємо за датою виконання — найнедавно виконані спочатку.
    /// </summary>
    public async Task<List<TodoItem>> GetArchivedTasksAsync()
    {
        return await _database
            .Table<TodoItem>()                                 // SELECT * FROM TodoItems
            .Where(t => t.IsCompleted)                         // WHERE IsCompleted = 1
            .OrderByDescending(t => t.CompletedAt)             // ORDER BY CompletedAt DESC
            .ToListAsync();
    }

    /// <summary>
    /// Додати нову справу до бази даних.
    ///
    /// InsertAsync — генерує SQL: INSERT INTO TodoItems (...) VALUES (...)
    /// Після виконання SQLite автоматично заповнює item.Id (завдяки [AutoIncrement]).
    /// </summary>
    /// <param name="item">Нова справа для збереження</param>
    /// <returns>Кількість вставлених рядків (зазвичай 1)</returns>
    public async Task<int> AddTaskAsync(TodoItem item)
    {
        return await _database.InsertAsync(item);
    }

    /// <summary>
    /// Оновити існуючу справу в базі даних.
    ///
    /// UpdateAsync — генерує SQL: UPDATE TodoItems SET ... WHERE Id = ?
    /// Пошук запису відбувається за полем Id (первинний ключ).
    ///
    /// Використовується для:
    /// - Позначення справи як виконаної (IsCompleted = true)
    /// - Редагування назви або опису
    /// </summary>
    /// <param name="item">Справа з оновленими полями (Id залишається незмінним)</param>
    /// <returns>Кількість оновлених рядків</returns>
    public async Task<int> UpdateTaskAsync(TodoItem item)
    {
        return await _database.UpdateAsync(item);
    }

    /// <summary>
    /// Видалити справу з бази даних назавжди.
    ///
    /// DeleteAsync — генерує SQL: DELETE FROM TodoItems WHERE Id = ?
    /// Дія незворотна — відновити видалений запис неможливо.
    /// </summary>
    /// <param name="item">Справа для видалення (використовується її Id)</param>
    /// <returns>Кількість видалених рядків</returns>
    public async Task<int> DeleteTaskAsync(TodoItem item)
    {
        return await _database.DeleteAsync(item);
    }
}
