using SQLite;

// Простір імен для моделей (класів даних) проєкту
namespace TodoApp.Model;

/// <summary>
/// Модель справи (завдання) у додатку.
/// Цей клас описує структуру одного запису в базі даних SQLite.
/// Атрибути SQLite вказують, як клас відображається на таблицю в БД.
/// </summary>
[Table("TodoItems")] // Назва таблиці в SQLite (якщо не вказати — використовується ім'я класу)
public class TodoItem
{
    /// <summary>
    /// Унікальний ідентифікатор справи.
    /// [PrimaryKey] — це первинний ключ таблиці (кожен запис унікальний).
    /// [AutoIncrement] — SQLite автоматично присвоює наступне число (1, 2, 3...).
    /// Нам не потрібно задавати Id вручну при створенні справи.
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Назва справи — короткий заголовок.
    /// [MaxLength(200)] — обмежуємо довжину рядка до 200 символів.
    /// NotNull означає, що поле обов'язкове (задається за замовчуванням = пустий рядок).
    /// </summary>
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Опис справи — детальніша інформація.
    /// Може бути порожнім (опціональне поле).
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Чи виконана справа?
    /// false = активна (на головній сторінці)
    /// true = виконана (переміщена в архів)
    /// </summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>
    /// Дата та час створення справи.
    /// Встановлюється автоматично при створенні нового запису.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Дата та час виконання справи.
    /// null = справа ще не виконана.
    /// Встановлюється при переміщенні до архіву.
    /// </summary>
    public DateTime? CompletedAt { get; set; }
}
