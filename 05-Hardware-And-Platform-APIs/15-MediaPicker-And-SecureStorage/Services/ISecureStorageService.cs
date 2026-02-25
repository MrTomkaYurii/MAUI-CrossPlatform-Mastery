namespace MauiMastery.MediaAndFileSystem.Services;

public interface ISecureStorageService
{
    Task SetAsync(string key, string value);
    Task<string?> GetAsync(string key);
    bool Remove(string key);
}

public class InMemorySecureStorageService : ISecureStorageService
{
    private readonly Dictionary<string, string> _store = new();

    public Task SetAsync(string key, string value)
    {
        _store[key] = value;
        return Task.CompletedTask;
    }

    public Task<string?> GetAsync(string key)
    {
        _store.TryGetValue(key, out var val);
        return Task.FromResult(val);
    }

    public bool Remove(string key) => _store.Remove(key);
}