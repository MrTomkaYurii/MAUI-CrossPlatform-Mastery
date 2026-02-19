using SQLite;

namespace MauiMastery.SQLiteDatabase.Models;

[Table("Notes")]
public class NoteRecord
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(150), NotNull]
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}