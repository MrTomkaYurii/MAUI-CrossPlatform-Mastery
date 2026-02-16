using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiMastery.InfiniteScrollRefresh.ViewModels;

public class PagedFeedViewModel : INotifyPropertyChanged
{
    private bool _isRefreshing;
    private int _currentPage = 1;

    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<string> FeedItems { get; } = new();

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set { if (_isRefreshing != value) { _isRefreshing = value; OnPropertyChanged(); } }
    }

    public PagedFeedViewModel()
    {
        LoadInitial();
    }

    public void LoadInitial()
    {
        FeedItems.Clear();
        _currentPage = 1;
        for (int i = 1; i <= 10; i++)
        {
            FeedItems.Add($"Елемент #{i} (Сторінка 1)");
        }
    }

    public async Task RefreshAsync()
    {
        IsRefreshing = true;
        await Task.Delay(50);
        LoadInitial();
        IsRefreshing = false;
    }

    public async Task LoadNextPageAsync()
    {
        _currentPage++;
        await Task.Delay(50);
        int start = (_currentPage - 1) * 10 + 1;
        for (int i = start; i < start + 10; i++)
        {
            FeedItems.Add($"Елемент #{i} (Сторінка {_currentPage})");
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}