using MauiMastery.SQLiteDatabase.Models;

namespace MauiMastery.SQLiteDatabase.Repositories;

public interface INoteRepository
{
    Task<List<NoteRecord>> GetAllAsync();
    Task<NoteRecord?> GetByIdAsync(int id);
    Task<int> SaveAsync(NoteRecord note);
    Task<int> DeleteAsync(NoteRecord note);
}

public class InMemoryNoteRepository : INoteRepository
{
    private readonly List<NoteRecord> _notes = new();
    private int _idCounter = 1;

    public Task<List<NoteRecord>> GetAllAsync() => Task.FromResult(_notes.ToList());
    public Task<NoteRecord?> GetByIdAsync(int id) => Task.FromResult(_notes.FirstOrDefault(n => n.Id == id));

    public Task<int> SaveAsync(NoteRecord note)
    {
        if (note.Id == 0)
        {
            note.Id = _idCounter++;
            _notes.Add(note);
        }
        else
        {
            var idx = _notes.FindIndex(n => n.Id == note.Id);
            if (idx >= 0) _notes[idx] = note;
        }
        return Task.FromResult(1);
    }

    public Task<int> DeleteAsync(NoteRecord note)
    {
        var removed = _notes.RemoveAll(n => n.Id == note.Id);
        return Task.FromResult(removed);
    }
}